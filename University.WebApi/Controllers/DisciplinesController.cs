using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Roles;
using System.Security.Claims;
using System;
using University.WebApi.Contexts;
using University.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Models.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace University.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisciplinesController : ControllerBase
    {
        private AppDbContext _appDbContext;
        private IAuthService _authService;
        private UserManager<ApplicationUser> userManager;


        public DisciplinesController(AppDbContext appDbContext, IAuthService authService, UserManager<ApplicationUser> userManager)
        {
            _appDbContext = appDbContext;
            _authService = authService;
            this.userManager = userManager;
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<Discipline>>> GetListAsync()
        {
            return await _appDbContext.Disciplines.ToListAsync();
        }

        // GET: api/<DisciplinesController>
        [Authorize(Roles = $"{Roles.HeadOfDepartment},{Roles.GuarantorOfSpeciality}")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null) { return NotFound("User not found!"); }
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null) { return NotFound("User not found!"); }

            var userRoles = await userManager.GetRolesAsync(user);
            int departmentId = -1;

            switch (userRoles.First())
            {
                case Roles.HeadOfDepartment:
                    departmentId = _appDbContext.HeadsOfDepartments
                        .First(h => h.ApplicationUser == user).DepartmentId;                    
                    break;                
            }

            var query = _appDbContext.LecturerDisciplines
                .Where(ld => ld.Lecturer.Departments.Any(d => d.DepartmentId == departmentId)) // Filter by department
                .Select(ld => ld.Discipline) // Select disciplines
                .Distinct() // Get distinct disciplines
                .ToList(); // Convert the result to a list


            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy() // Use camelCase for property names
                },
                Formatting = Formatting.Indented, // Indent the JSON for readability
                NullValueHandling = NullValueHandling.Ignore, // Ignore null values during serialization
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // Ignore reference loops
            };

            // Serialize the query results with custom serialization settings
            var jsonResult = JsonConvert.SerializeObject(query.ToList(), serializerSettings);

            return Ok(jsonResult);
        }

        // GET api/<DisciplinesController>/5
        [Authorize(Roles = "HeadOfDepartment")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = _appDbContext.Disciplines.Include(d => d.Publications.Where(p => p.isPublished))
                    .ThenInclude(p => p.Authors)
                .FirstOrDefault(d => d.Id == id);

            if (query == null)
                return NotFound();

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy() // Use camelCase for property names
                },
                Formatting = Formatting.Indented, // Indent the JSON for readability
                NullValueHandling = NullValueHandling.Ignore, // Ignore null values during serialization
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // Ignore reference loops
            };

            var jsonResult = JsonConvert.SerializeObject(query, serializerSettings);

            return Ok(jsonResult);
        }

        // POST api/<DisciplinesController>
        [Authorize(Roles = Roles.HeadOfDepartment)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string disciplineName)
        {
            _appDbContext.Disciplines.Add(new Discipline { Name = disciplineName });
            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        // PUT api/<DisciplinesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DisciplinesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
