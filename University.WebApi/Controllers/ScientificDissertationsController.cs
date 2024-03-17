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
    public class ScientificDissertationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public ScientificDissertationsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/ScientificDissertations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScientificDissertation>>> GetscientificDissertations()
        {
            return await _context.ScientificDissertations.ToListAsync();
        }

        // GET: api/ScientificDissertations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScientificDissertation>> GetScientificDissertation(int id)
        {
            var scientificDissertation = await _context.ScientificDissertations.FindAsync(id);

            if (scientificDissertation == null)
            {
                return NotFound();
            }

            return scientificDissertation;
        }

        // PUT: api/ScientificDissertations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScientificDissertation(int id, ScientificDissertation scientificDissertation)
        {
            if (id != scientificDissertation.PublicationId)
            {
                return BadRequest();
            }

            _context.Entry(scientificDissertation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScientificDissertationExists(id))
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

        // POST: api/ScientificDissertations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = $"{Roles.Lecturer}, {Roles.HeadOfDepartment}")]
        [HttpPost]
        public async Task<ActionResult<ScientificDissertation>> PostScientificDissertation(PostScientificDissertationDto scientificPublicationDto)
        {
            ScientificDissertation entity = _mapper.Map<ScientificDissertation>(scientificPublicationDto);

            entity.PublicationDate = DateTime.SpecifyKind(entity.PublicationDate.Value, DateTimeKind.Utc);

            var authors = await _context.Persons.Where(p => scientificPublicationDto.AuthorIds.Contains(p.Id)).ToListAsync();
            entity.Authors = new List<Person>();
            entity.Authors.AddRange(authors);

            var disciplines = await _context.Disciplines.Where(d => scientificPublicationDto.DisciplinesIds.Contains(d.Id)).ToListAsync();
            entity.Disciplines = new List<Discipline>();
            entity.Disciplines.AddRange(disciplines);

            _context.ScientificDissertations.Add(entity);
            await _context.SaveChangesAsync();

            var json = JsonConvert.SerializeObject(entity, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });


            return CreatedAtAction("GetScientificDissertation", new { id = entity.PublicationId }, json);
        }

        // DELETE: api/ScientificDissertations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScientificDissertation(int id)
        {
            var scientificDissertation = await _context.ScientificDissertations.FindAsync(id);
            if (scientificDissertation == null)
            {
                return NotFound();
            }

            _context.ScientificDissertations.Remove(scientificDissertation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ScientificDissertationExists(int id)
        {
            return _context.ScientificDissertations.Any(e => e.PublicationId == id);
        }
    }
}
