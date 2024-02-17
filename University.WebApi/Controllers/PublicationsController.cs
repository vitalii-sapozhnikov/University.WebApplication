using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.WebApi.Dtos;
using Models.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using University.WebApi.Contexts;
using University.WebApi.Mapping;
using University.WebApi.Dtos.MethodologicalPublicationDto;
using AutoMapper;

namespace University.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicationsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public PublicationsController(AppDbContext appDbContext, UserManager<ApplicationUser> userManager, IMapper mappper)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _mapper = mappper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            string? searchTerm,
            string? authorName,
            DateTime? startDateFilter,
            DateTime? endDateFilter)
        {
            IQueryable<Publication> query = _appDbContext.Publications.Include(p => p.Authors)
                .Where(p => p.isPublished == true);

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
                var lastName = authorName.Split(' ')[0];
                var firstName = authorName.Split(' ')[1];
                var middleName = authorName.Split(' ')[2];
                middleName = middleName.Remove(middleName.Length - 1);

                // Filter by Author name
                query = query.Where(p => p.Authors.Any(a => 
                    EF.Functions.Like(a.FirstName, $"%{firstName}%") &&
                    EF.Functions.Like(a.LastName, $"%{lastName}%") &&
                    EF.Functions.Like(a.MiddleName, $"%{middleName}%")));
            }

            if (startDateFilter.HasValue)
            {
                startDateFilter = DateTime.SpecifyKind(startDateFilter.Value, DateTimeKind.Utc);
                query = query.Where(p => p.PublicationDate >= startDateFilter.Value.Date);
            }

            if (endDateFilter.HasValue)
            {
                endDateFilter = DateTime.SpecifyKind(endDateFilter.Value, DateTimeKind.Utc);
                query = query.Where(p => p.PublicationDate <= endDateFilter.Value.Date);
            }

            var publicationShortDtos = await query
                .Take(100)
                .Select(item => new PublicationShortDto
                {
                    PublicationId = item.PublicationId,
                    Title = item.Title,
                    PublicationDate = item.PublicationDate ?? DateTime.Now.ToUniversalTime(),
                    Description = item.Description,
                    Keywords = item.Keywords,
                    Lecturers = item.Authors
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
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                };

                string json = System.Text.Json.JsonSerializer.Serialize(publication, options);

                return Ok(json);
            }
            return NotFound();
        }

        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(UploadMethodologicalPublicationDto publicationDto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var publication = _mapper.Map<MethodologicalPublication>(publicationDto);

            if(publicationDto.AuthorIds.Any())
            {
                publication.Authors = new List<Person>();

                foreach (var lecturerId in publicationDto.AuthorIds)
                {
                    var lecturer = await _appDbContext.Lecturers.FirstOrDefaultAsync(a => a.Id == lecturerId);

                    if (lecturer == null) { return BadRequest(ModelState); }

                    // Add the author to the publication
                    publication.Authors.Add(lecturer);
                }
            }

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null) { return NotFound("User not found!"); }
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound("User not found!");
            }

            // TODO: ADD AUTHOR TO A PUBLICATION

            if (publication.PublicationDate.HasValue)
            {
                publication.PublicationDate = DateTime.SpecifyKind(publication.PublicationDate.Value, DateTimeKind.Utc);
            }

            await _appDbContext.Publications.AddAsync(publication);

            await _appDbContext.SaveChangesAsync();

            return Ok("Successfully added");
        }

        [Authorize]
        [HttpPost("modify/{id}")]
        public async Task<IActionResult> Modify(MethodologicalPublicationDto model, int id)
        {
            var publication = _appDbContext.MethodologicalPublications.Include(p => p.Authors).First(p => p.PublicationId == id);
            if (!string.IsNullOrEmpty(model.Title))
            {
                publication.Title = model.Title;
            }

            if(model.PublicationDate.HasValue)
            {
                publication.PublicationDate = DateTime.SpecifyKind(model.PublicationDate.Value, DateTimeKind.Utc);
            }

            if (!string.IsNullOrEmpty(model.CloudStorageGuid))
            {
                publication.CloudStorageGuid = model.CloudStorageGuid;
            }

            if (!string.IsNullOrEmpty(model.Abstract))
            {
                publication.Abstract = model.Abstract;
            }

            if (!string.IsNullOrEmpty(model.Description))
            {
                publication.Description = model.Description;
            }

            if(model.Keywords != null)
            {
                publication.Keywords = model.Keywords;
            }

            publication.isPublished = model.isPublished;

            if(model.Volume.HasValue)
            {
                publication.Volume = model.Volume.Value;
            }

            if (model.Language.HasValue)
            {
                publication.Language = model.Language.Value;
            }

            if (model.Type.HasValue)
            {
                publication.Type = model.Type.Value;
            }

            if(model.LecturerIds != null)
            {
                publication.Authors.Clear();

                var lecturersToAdd = _appDbContext.Lecturers
                    .Where(l => model.LecturerIds.Contains(l.Id))
                    .Select(l => l as Person)
                    .ToList();

                publication.Authors = lecturersToAdd;
            }

            await _appDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
