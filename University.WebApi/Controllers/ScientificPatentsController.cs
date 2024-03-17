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
    public class ScientificPatentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ScientificPatentsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/ScientificPatents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScientificPatent>>> GetScientificPatents()
        {
            return await _context.ScientificPatents.ToListAsync();
        }

        // GET: api/ScientificPatents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScientificPatent>> GetScientificPatent(int id)
        {
            var scientificPatent = await _context.ScientificPatents.FindAsync(id);

            if (scientificPatent == null)
            {
                return NotFound();
            }

            return scientificPatent;
        }

        // PUT: api/ScientificPatents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScientificPatent(int id, ScientificPatent scientificPatent)
        {
            if (id != scientificPatent.PublicationId)
            {
                return BadRequest();
            }

            _context.Entry(scientificPatent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScientificPatentExists(id))
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

        // POST: api/ScientificPatents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = $"{Roles.Lecturer}, {Roles.HeadOfDepartment}")]
        [HttpPost]
        public async Task<ActionResult<ScientificPatent>> PostScientificPatent(PostScientificPatentDto scientificPublicationDto)
        {
            ScientificPatent entity = _mapper.Map<ScientificPatent>(scientificPublicationDto);

            entity.PublicationDate = DateTime.SpecifyKind(entity.PublicationDate.Value, DateTimeKind.Utc);

            var authors = await _context.Persons.Where(p => scientificPublicationDto.AuthorIds.Contains(p.Id)).ToListAsync();
            entity.Authors = new List<Person>();
            entity.Authors.AddRange(authors);

            var disciplines = await _context.Disciplines.Where(d => scientificPublicationDto.DisciplinesIds.Contains(d.Id)).ToListAsync();
            entity.Disciplines = new List<Discipline>();
            entity.Disciplines.AddRange(disciplines);

            _context.ScientificPatents.Add(entity);
            await _context.SaveChangesAsync();

            var json = JsonConvert.SerializeObject(entity, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            return CreatedAtAction("GetScientificPatent", new { id = entity.PublicationId }, json);
        }

        // DELETE: api/ScientificPatents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScientificPatent(int id)
        {
            var scientificPatent = await _context.ScientificPatents.FindAsync(id);
            if (scientificPatent == null)
            {
                return NotFound();
            }

            _context.ScientificPatents.Remove(scientificPatent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ScientificPatentExists(int id)
        {
            return _context.ScientificPatents.Any(e => e.PublicationId == id);
        }
    }
}
