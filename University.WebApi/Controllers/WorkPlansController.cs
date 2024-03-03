using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.Roles;
using Newtonsoft.Json;
using University.WebApi.Contexts;
using University.WebApi.Dtos.WorkPlanDtos;

namespace University.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkPlansController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public WorkPlansController(AppDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: api/WorkPlans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkPlan>>> GetWorkPlans()
        {
            return await _context.WorkPlans.ToListAsync();
        }

        // GET: api/WorkPlans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkPlan>> GetWorkPlan(int id)
        {
            var workPlan = await _context.WorkPlans.FindAsync(id);

            if (workPlan == null)
            {
                return NotFound();
            }

            return workPlan;
        }

        // PUT: api/WorkPlans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkPlan(int id, WorkPlan workPlan)
        {
            if (id != workPlan.Id)
            {
                return BadRequest();
            }

            _context.Entry(workPlan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkPlanExists(id))
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

        // POST: api/WorkPlans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WorkPlan>> PostWorkPlan(PostWorkPlan workPlanDto)
        {
            var workPlan = _mapper.Map<WorkPlan>(workPlanDto);
            workPlan.Disciplines = _context.Disciplines.Where(d => workPlanDto.DisciplinesIds.Contains(d.Id)).ToList();


            _context.WorkPlans.Add(workPlan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkPlan", new { id = workPlan.Id }, workPlan);
        }

        // DELETE: api/WorkPlans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkPlan(int id)
        {
            var workPlan = await _context.WorkPlans.FindAsync(id);
            if (workPlan == null)
            {
                return NotFound();
            }

            _context.WorkPlans.Remove(workPlan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = Roles.GuarantorOfSpeciality)]
        [HttpGet("guarantor")]
        public async Task<IActionResult> GetWorkPlansForGuarantor()
        {
            string userEmail = User.FindFirstValue(ClaimTypes.Email) ?? "";
            ApplicationUser? user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null) { return BadRequest(); }

            var workPlan = await _context.WorkPlans
                .Include(wp => wp.Speciality)
                .Include(wp => wp.EducationYear)
                .Include(wp => wp.Department)
                .Include(wp => wp.Disciplines)
                    .ThenInclude(d => d.LecturerDisciplines)
                        .ThenInclude(ld => ld.Lecturer)
                .Where(wp => wp.Guarantor == user).ToListAsync();

            var workPlanDtos = workPlan.Select(wp => _mapper.Map<GetWorkPlan>(wp)).ToList();

            var settings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

            string json = JsonConvert.SerializeObject(workPlanDtos, Formatting.Indented, settings);

            return Ok(json);
        }

        private bool WorkPlanExists(int id)
        {
            return _context.WorkPlans.Any(e => e.Id == id);
        }
    }
}
