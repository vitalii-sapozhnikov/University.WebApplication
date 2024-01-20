using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Dtos;
using University.WebApi.Contexts;
using University.WebApi.Mapping;
using University.WebApi.Models;

namespace University.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public AuthorsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpPost("add-multiple")]
        public async Task<IActionResult> AddMultipleAuthors(List<AddAuthorDto> authorDtos)
        {
            try
            {
                if (authorDtos == null || !authorDtos.Any())
                {
                    return BadRequest("No authors provided.");
                }

                List<Author> authors = authorDtos.Select(a => a.ToAuthorEntity()).ToList();

                // Add the authors to the database
                _dbContext.Authors.AddRange(authors);
                await _dbContext.SaveChangesAsync();

                return Ok("Authors added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAuthor(AddAuthorDto authorDto)
        {
            try
            {
                if (authorDto == null)
                {
                    return BadRequest("No author provided.");
                }

                _dbContext.Authors.Add(authorDto.ToAuthorEntity());
                await _dbContext.SaveChangesAsync();

                return Ok("Author added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAuthors()
        {
            try
            {
                var authors = await _dbContext.Authors.ToListAsync();
                var authorDtos = authors.Select(a => a.ToAuthorDto());
                return Ok(authorDtos);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
