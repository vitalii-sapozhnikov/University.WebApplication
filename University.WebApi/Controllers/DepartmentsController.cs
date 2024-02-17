using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.WebApi.Contexts;

namespace University.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : Controller
    {
        private AppDbContext _appDbContext;

        public DepartmentsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var deps = await _appDbContext.Departments.ToListAsync();
            return Ok(deps);
        }
    }
}
