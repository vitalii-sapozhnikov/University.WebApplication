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
        public async Task<IActionResult> Get()
        {
            return Ok();
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
