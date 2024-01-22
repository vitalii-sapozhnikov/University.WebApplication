using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Dtos;
using Models.Models;
using System.Security.Claims;
using University.WebApi.Contexts;
using University.WebApi.Mapping;
using University.WebApi.Models;

namespace University.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicationsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private UserManager<ApplicationUser> _userManager;

        public PublicationsController(AppDbContext appDbContext, UserManager<ApplicationUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            string? searchTerm,
            string? authorName,
            DateTime? startDateFilter,
            DateTime? endDateFilter)
        {
            IQueryable<Publication> query = _appDbContext.Publications.Include(p => p.Authors);

            // Apply filters based on the provided parameters
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Fuzzy search for Title, Description, Abstract, and Keywords
                query = query.Where(p =>
                    EF.Functions.Like(p.Title.ToLower(), $"%{searchTerm.ToLower()}%") ||
                    EF.Functions.Like(p.Description.ToLower(), $"%{searchTerm.ToLower()}%") ||
                    EF.Functions.Like(p.Abstract.ToLower(), $"%{searchTerm.ToLower()}%") ||
                    p.Keywords.Any(k => EF.Functions.Like(k.ToLower(), $"%{searchTerm.ToLower()}%")));

            }

            if (!string.IsNullOrEmpty(authorName))
            {
                // Filter by Author name
                query = query.Where(p => p.Authors
                    .Any(a => EF.Functions
                    .Like($"{a.LastName} {a.FirstName} {a.MiddleName}", $"%{authorName}%")));
            }

            if (startDateFilter.HasValue)
            {
                query = query.Where(p => p.PublicationDate >= startDateFilter.Value.Date);
            }

            if (endDateFilter.HasValue)
            {
                query = query.Where(p => p.PublicationDate <= endDateFilter.Value.Date);
            }

            var publicationShortDtos = await query
                .Take(100)
                .Select(item => new PublicationShortDto
                {
                    PublicationId = item.PublicationId,
                    Title = item.Title,
                    PublicationDate = item.PublicationDate,
                    Description = item.Description,
                    Keywords = item.Keywords,
                    Authors = item.Authors
                })
                .ToListAsync();

            return Ok(publicationShortDtos);
        }



        [HttpGet]
        [Route("info/{idx}")]
        public async Task<IActionResult> GetInfo(int idx)
        {
            var publication = await _appDbContext.Publications
                .Include(p => p.Authors)
                .FirstOrDefaultAsync(p => p.PublicationId == idx);

            if(publication != null)
            {
                return Ok(publication);
            }
            return NotFound();
        }

        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(UploadPublicationDto publicationDto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var publication = publicationDto.ToPublicationEntity();

            if(publicationDto.AuthorIds.Any())
            {
                publication.Authors = new List<Author>();

                foreach (var authorId in publicationDto.AuthorIds)
                {
                    var author = await _appDbContext.Authors.FirstOrDefaultAsync(a => a.AuthorId == authorId);

                    if (author == null) { return BadRequest(ModelState); }

                    // Add the author to the publication
                    publication.Authors.Add(author);
                }
            }

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null) { return NotFound("User not found!"); }
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound("User not found!");
            }

            publication.User = user;

            publication.PublicationDate = DateTime.SpecifyKind(publication.PublicationDate, DateTimeKind.Utc);

            await _appDbContext.Publications.AddAsync(publication);

            await _appDbContext.SaveChangesAsync();

            return Ok("Successfully added");
        }
    }
}
