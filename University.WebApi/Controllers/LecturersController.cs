using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using University.WebApi.Contexts;
using University.WebApi.Mapping;
using Models.Models;
using University.WebApi.Dtos.PersonDtos;

namespace University.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public LecturersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpPost("add-multiple")]
        public async Task<IActionResult> AddMultipleLecturers(List<PostPersonDto> lecturerDtos)
        {
            try
            {
                if (lecturerDtos == null || !lecturerDtos.Any())
                {
                    return BadRequest("No lecturers provided.");
                }

                List<Lecturer> lecturers = lecturerDtos.Select(a =>
                    {
                        var lecturer = a.ToLecturerEntity();
                        //TODO: Lecturer Department when adding lecturer
                        //lecturer.DepartmentId = 0;
                        return lecturer;
                    }).ToList();

                // Add the authors to the database
                _dbContext.Lecturers.AddRange(lecturers);
                await _dbContext.SaveChangesAsync();

                return Ok("Lecturers added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddLecturer(PostPersonDto lecturerDto)
        {
            try
            {
                if (lecturerDto == null)
                {
                    return BadRequest("No lecturer provided.");
                }
                Lecturer lecturer = lecturerDto.ToLecturerEntity();
                
                //TODO: Lecturer department when added
                //lecturer.DepartmentId = 0;

                _dbContext.Lecturers.Add(lecturer);
                await _dbContext.SaveChangesAsync();

                return Ok("Lecturer added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllLecturers()
        {
            try
            {
                var lecturers = await _dbContext.Lecturers.Include(l => l.Departments).ToListAsync();
                var lecturerDtos = lecturers.Select(a => a.ToLecturerDto());

                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                var json = JsonConvert.SerializeObject(lecturerDtos, Formatting.Indented, settings);
                return Ok(json);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetLecturer(int id)
        {
            var author = await _dbContext.Lecturers
                .Include(a => a.Publications)
                .FirstOrDefaultAsync(a => a.Id == id);

            author.Publications = author.Publications.Where(p => p.isPublished).ToList();
            
            if (author is null)
                return NotFound();

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            return Ok(JsonConvert.SerializeObject(author, jsonSettings));
        }

        [HttpGet("correlation")]
        public async Task<IActionResult> GetLecturerCorrelation(int lectId, int discId)
        {
            var lecturer = await _dbContext.Lecturers
                .Include(l => l.Publications) // Include the publications
                    .ThenInclude(p => p.Disciplines) // Include the disciplines for each publication
                .FirstOrDefaultAsync(l => l.Id == lectId);

            if (lecturer == null) return NotFound();

            var publications = lecturer.Publications
                .Where(p => p.Disciplines.Any(d => d.Id == discId))
                .ToList();

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto
            };

            string json = JsonConvert.SerializeObject(publications, Formatting.Indented, settings);

            return Ok(json);
        }
    }
}
