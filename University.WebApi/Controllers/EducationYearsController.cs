using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using University.WebApi.Contexts;
using University.WebApi.Dtos.EducationalYearDtos;

namespace University.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationYearsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public EducationYearsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/EducationYears
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EducationYear>>> GetEducationYear()
        {
            return await _context.EducationYear.ToListAsync();
        }

        // GET: api/EducationYears/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EducationYear>> GetEducationYear(int id)
        {
            var educationYear = await _context.EducationYear.FindAsync(id);

            if (educationYear == null)
            {
                return NotFound();
            }

            return educationYear;
        }

        // PUT: api/EducationYears/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEducationYear(int id, EducationYear educationYear)
        {
            if (id != educationYear.Id)
            {
                return BadRequest();
            }

            _context.Entry(educationYear).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EducationYearExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EducationYears
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EducationYear>> PostEducationYear(AddYearDto educationYearDto)
        {
            var educationYear = _mapper.Map<EducationYear>(educationYearDto);
            _context.EducationYear.Add(educationYear);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEducationYear", new { id = educationYear.Id }, educationYear);
        }

        // DELETE: api/EducationYears/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEducationYear(int id)
        {
            var educationYear = await _context.EducationYear.FindAsync(id);
            if (educationYear == null)
            {
                return NotFound();
            }

            _context.EducationYear.Remove(educationYear);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EducationYearExists(int id)
        {
            return _context.EducationYear.Any(e => e.Id == id);
        }
    }
}
