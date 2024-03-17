using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.Roles;
using Newtonsoft.Json;
using University.WebApi.Contexts;
using University.WebApi.Dtos.ScientificPublicationDto;
using NuGet.Packaging;

namespace University.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScientificMonographsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ScientificMonographsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        // GET: api/ScientificMonographs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScientificMonograph>>> GetScientificMonographs()
        {
            return await _context.ScientificMonographs.ToListAsync();
        }

        // GET: api/ScientificMonographs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScientificMonograph>> GetScientificMonograph(int id)
        {
            var scientificMonograph = await _context.ScientificMonographs.FindAsync(id);

            if (scientificMonograph == null)
            {
                return NotFound();
            }

            return scientificMonograph;
        }

        // PUT: api/ScientificMonographs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScientificMonograph(int id, ScientificMonograph scientificMonograph)
        {
            if (id != scientificMonograph.PublicationId)
            {
                return BadRequest();
            }

            _context.Entry(scientificMonograph).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScientificMonographExists(id))
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

        // POST: api/ScientificMonographs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = $"{Roles.Lecturer}, {Roles.HeadOfDepartment}")]
        [HttpPost]
        public async Task<ActionResult<ScientificMonograph>> PostScientificMonograph(PostScientificMonographDto scientificPublicationDto)
        {
            ScientificMonograph entity = _mapper.Map<ScientificMonograph>(scientificPublicationDto);

            entity.PublicationDate = DateTime.SpecifyKind(entity.PublicationDate.Value, DateTimeKind.Utc);

            var authors = await _context.Persons.Where(p => scientificPublicationDto.AuthorIds.Contains(p.Id)).ToListAsync();
            entity.Authors = new List<Person>();
            entity.Authors.AddRange(authors);

            var disciplines = await _context.Disciplines.Where(d => scientificPublicationDto.DisciplinesIds.Contains(d.Id)).ToListAsync();
            entity.Disciplines = new List<Discipline>();
            entity.Disciplines.AddRange(disciplines);

            _context.ScientificMonographs.Add(entity);
            await _context.SaveChangesAsync();

            var json = JsonConvert.SerializeObject(entity, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            return CreatedAtAction("GetScientificMonographs", new { id = entity.PublicationId }, json);
        }

        // DELETE: api/ScientificMonographs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScientificMonograph(int id)
        {
            var scientificMonograph = await _context.ScientificMonographs.FindAsync(id);
            if (scientificMonograph == null)
            {
                return NotFound();
            }

            _context.ScientificMonographs.Remove(scientificMonograph);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ScientificMonographExists(int id)
        {
            return _context.ScientificMonographs.Any(e => e.PublicationId == id);
        }
    }
}
