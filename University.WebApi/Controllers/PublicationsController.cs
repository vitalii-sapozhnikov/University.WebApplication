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
using Models.Roles;
using University.WebApi.Services;
using iText.Commons.Actions.Contexts;
using NuGet.Packaging;
using static iText.IO.Util.IntHashtable;

namespace University.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicationsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IApplicationUserService _applicationUser;

        public PublicationsController(AppDbContext appDbContext, UserManager<ApplicationUser> userManager, IMapper mappper, IApplicationUserService applicationUserService)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _mapper = mappper;
            _applicationUser = applicationUserService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            string? searchTerm,
            string? authorName,
            DateTime? startDateFilter,
            DateTime? endDateFilter,
            string? categories)
        {
            IQueryable<Publication> query = _appDbContext.Publications.Include(p => p.Authors)
                .Where(p => p.isPublished == true);

            // Apply filters based on the provided parameters
            if (!string.IsNullOrEmpty(categories))
            {
                int[] categoryIds = categories?.Split(',').Select(int.Parse).ToArray();
                query = query.Where(p => p.Disciplines.Any(d => categoryIds.Contains(d.Id)));
            }
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

                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.All
                };

                return Ok(JsonConvert.SerializeObject(publication, Formatting.Indented, settings));
            }
            return NotFound();
        }

        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(UploadMethodologicalPublicationDto publicationDto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var publication = _mapper.Map<MethodologicalPublication>(publicationDto);

            var disciplines = await _appDbContext.Disciplines.Where(d => publicationDto.DisciplineIds.Contains(d.Id)).ToListAsync();
            publication.Disciplines = new List<Discipline>();
            publication.Disciplines.AddRange(disciplines);

            var authors = await _appDbContext.Persons.Where(p => publicationDto.AuthorIds.Contains(p.Id)).ToListAsync();
            publication.Authors = new List<Person>();
            publication.Authors.AddRange(authors);


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


        [Authorize(Roles = Roles.HeadOfDepartment)]
        [HttpGet("departmentalPublications")]
        public async Task<IActionResult> GetDepartmentalPublications()
        {            
            string userEmail = User.FindFirstValue(ClaimTypes.Email) ?? "";
            ApplicationUser? user = await _userManager.FindByEmailAsync(userEmail);

            var head = await _appDbContext.HeadsOfDepartments.FirstOrDefaultAsync(h => h.ApplicationUser == user);
            if(head == null) { return NotFound("User not found!"); }

            var publications = await _appDbContext.Lecturers
                .Where(l => l.Departments.Any(d => d.DepartmentId == head.DepartmentId))
                .SelectMany(l => l.Publications)
                .Where(p => p.isPublished)
                .Include(p => p.Authors)
                .ToListAsync();

            var settings = new JsonSerializerSettings 
            { 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto
            };

            return Ok(JsonConvert.SerializeObject(publications, Formatting.Indented, settings));
        }

        [Authorize(Roles = $"{Roles.HeadOfDepartment},{Roles.Lecturer}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var publication = _appDbContext.Publications
                                    .Include(p => p.Authors) 
                                    .FirstOrDefault(p => p.PublicationId == id);

            string userEmail = User.FindFirstValue(ClaimTypes.Email) ?? "";
            ApplicationUser? user = await _userManager.FindByEmailAsync(userEmail);

            if (publication != null && publication.Authors.Any(p => p.ApplicationUserId == user.Id))
            {
                _appDbContext.Publications.Remove(publication);
                await _appDbContext.SaveChangesAsync();
                return Ok("Publication deleted successfully.");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
