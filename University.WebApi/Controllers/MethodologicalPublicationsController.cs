using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using University.WebApi.Contexts;

namespace University.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MethodologicalPublicationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MethodologicalPublicationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MethodologicalPublications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MethodologicalPublication>>> GetMethodologicalPublications()
        {
            return await _context.MethodologicalPublications.ToListAsync();
        }

        // GET: api/MethodologicalPublications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MethodologicalPublication>> GetMethodologicalPublication(int id)
        {
            var methodologicalPublication = await _context.MethodologicalPublications.FindAsync(id);

            if (methodologicalPublication == null)
            {
                return NotFound();
            }

            return methodologicalPublication;
        }

        // PUT: api/MethodologicalPublications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMethodologicalPublication(int id, MethodologicalPublication methodologicalPublication)
        {
            if (id != methodologicalPublication.PublicationId)
            {
                return BadRequest();
            }

            _context.Entry(methodologicalPublication).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MethodologicalPublicationExists(id))
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

        // POST: api/MethodologicalPublications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MethodologicalPublication>> PostMethodologicalPublication(MethodologicalPublication methodologicalPublication)
        {
            _context.MethodologicalPublications.Add(methodologicalPublication);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMethodologicalPublication", new { id = methodologicalPublication.PublicationId }, methodologicalPublication);
        }

        // DELETE: api/MethodologicalPublications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMethodologicalPublication(int id)
        {
            var methodologicalPublication = await _context.MethodologicalPublications.FindAsync(id);
            if (methodologicalPublication == null)
            {
                return NotFound();
            }

            _context.MethodologicalPublications.Remove(methodologicalPublication);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MethodologicalPublicationExists(int id)
        {
            return _context.MethodologicalPublications.Any(e => e.PublicationId == id);
        }
    }
}
