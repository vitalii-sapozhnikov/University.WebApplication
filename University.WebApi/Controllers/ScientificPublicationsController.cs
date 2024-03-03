using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Humanizer;
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
    public class ScientificPublicationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ScientificPublicationsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/ScientificPublications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScientificPublication>>> GetScientificPublications()
        {
            return await _context.ScientificPublications.ToListAsync();
        }

        // GET: api/ScientificPublications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScientificPublication>> GetScientificPublication(int id)
        {
            var scientificPublication = await _context.ScientificPublications.FindAsync(id);

            if (scientificPublication == null)
            {
                return NotFound();
            }

            return scientificPublication;
        }

        // PUT: api/ScientificPublications/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScientificPublication(int id, ScientificPublication scientificPublication)
        {
            if (id != scientificPublication.PublicationId)
            {
                return BadRequest();
            }

            _context.Entry(scientificPublication).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScientificPublicationExists(id))
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

        // POST: api/ScientificPublications
        [Authorize(Roles = $"{Roles.Lecturer}, {Roles.HeadOfDepartment}")]
        [HttpPost]
        public async Task<ActionResult<ScientificPublication>> PostScientificPublication(PostScientificPublicationDto scientificPublicationDto)
        {
            ScientificPublication entity = _mapper.Map<ScientificPublication>(scientificPublicationDto);

            entity.PublicationDate = DateTime.SpecifyKind(entity.PublicationDate.Value, DateTimeKind.Utc);

            var authors = await _context.Persons.Where(p => scientificPublicationDto.AuthorIds.Contains(p.Id)).ToListAsync();
            entity.Authors = new List<Person>();
            entity.Authors.AddRange(authors);

            var disciplines = await _context.Disciplines.Where(d => scientificPublicationDto.DisciplinesIds.Contains(d.Id)).ToListAsync();
            entity.Disciplines = new List<Discipline>();
            entity.Disciplines.AddRange(disciplines);

            _context.ScientificPublications.Add(entity);
            await _context.SaveChangesAsync();

            var json = JsonConvert.SerializeObject(entity, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore});

            return CreatedAtAction("GetScientificPublication", new { id = entity.PublicationId }, json);
        }

        // DELETE: api/ScientificPublications/5
        [Authorize(Roles = $"{Roles.Lecturer}, {Roles.HeadOfDepartment}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScientificPublication(int id)
        {
            var scientificPublication = await _context.ScientificPublications.FindAsync(id);
            if (scientificPublication == null)
            {
                return NotFound();
            }

            _context.ScientificPublications.Remove(scientificPublication);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ScientificPublicationExists(int id)
        {
            return _context.ScientificPublications.Any(e => e.PublicationId == id);
        }
    }
}
