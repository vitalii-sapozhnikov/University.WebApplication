using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.Roles;
using Newtonsoft.Json;
using NuGet.Packaging;
using University.WebApi.Contexts;
using University.WebApi.Dtos.ScientificPublicationDto;

namespace University.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScientificConferenceThesesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ScientificConferenceThesesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/ScientificConferenceTheses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScientificConferenceTheses>>> GetScientificConferenceTheses()
        {
            return await _context.ScientificConferenceTheses.ToListAsync();
        }

        // GET: api/ScientificConferenceTheses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScientificConferenceTheses>> GetScientificConferenceTheses(int id)
        {
            var scientificConferenceTheses = await _context.ScientificConferenceTheses.FindAsync(id);

            if (scientificConferenceTheses == null)
            {
                return NotFound();
            }

            return scientificConferenceTheses;
        }

        // PUT: api/ScientificConferenceTheses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScientificConferenceTheses(int id, ScientificConferenceTheses scientificConferenceTheses)
        {
            if (id != scientificConferenceTheses.PublicationId)
            {
                return BadRequest();
            }

            _context.Entry(scientificConferenceTheses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScientificConferenceThesesExists(id))
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

        // POST: api/ScientificConferenceTheses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = $"{Roles.Lecturer}, {Roles.HeadOfDepartment}")]
        [HttpPost]
        public async Task<ActionResult<ScientificConferenceTheses>> PostScientificConferenceTheses(PostScientificConferenceThesesDto scientificPublicationDto)
        {
            ScientificConferenceTheses entity = _mapper.Map<ScientificConferenceTheses>(scientificPublicationDto);

            entity.PublicationDate = DateTime.SpecifyKind(entity.PublicationDate.Value, DateTimeKind.Utc);

            var authors = await _context.Persons.Where(p => scientificPublicationDto.AuthorIds.Contains(p.Id)).ToListAsync();
            entity.Authors = new List<Person>();
            entity.Authors.AddRange(authors);

            var disciplines = await _context.Disciplines.Where(d => scientificPublicationDto.DisciplinesIds.Contains(d.Id)).ToListAsync();
            entity.Disciplines = new List<Discipline>();
            entity.Disciplines.AddRange(disciplines);

            _context.ScientificConferenceTheses.Add(entity);
            await _context.SaveChangesAsync();

            var json = JsonConvert.SerializeObject(entity, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            return CreatedAtAction("GetScientificConferenceTheses", new { id = entity.PublicationId }, json);
        }

        // DELETE: api/ScientificConferenceTheses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScientificConferenceTheses(int id)
        {
            var scientificConferenceTheses = await _context.ScientificConferenceTheses.FindAsync(id);
            if (scientificConferenceTheses == null)
            {
                return NotFound();
            }

            _context.ScientificConferenceTheses.Remove(scientificConferenceTheses);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ScientificConferenceThesesExists(int id)
        {
            return _context.ScientificConferenceTheses.Any(e => e.PublicationId == id);
        }
    }
}
