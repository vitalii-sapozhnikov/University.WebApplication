using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.Roles;
using University.Web.Services;
using University.Web.Services.Contracts;

namespace University.Web.Controllers
{
    [Route("[controller]")]
    public class DisciplinesController : Controller
    {
        private readonly IApiService _apiService;

        public DisciplinesController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [AuthorizeSession(Roles.HeadOfDepartment,Roles.GuarantorOfSpeciality)]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var disciplinesResult = await _apiService.GetDisciplinesAsync();
            if(!disciplinesResult.Success)
                return BadRequest(disciplinesResult.ErrorMessage);

            var departmetnIdResult = await _apiService.GetCurrentDepartmentId();
            var departments = await _apiService.GetDepartmentsAsync();

            if (disciplinesResult.Success && departmetnIdResult.Success && departments.Success)
            {
                var departmentName = departments.Data.First(d => d.DepartmentId == departmetnIdResult.Data).Name;
                return View((disciplinesResult.Data, departmentName));
            }

            return BadRequest();
        }

        [AuthorizeSession(Roles.HeadOfDepartment,Roles.GuarantorOfSpeciality)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id)
        {
            var result = await _apiService.GetDisciplineAsync(id);
            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return View("Info", result.Data);
        }

        [HttpPost("")]
        public async Task<IActionResult> Post(string disciplineName)
        {
            var result = await _apiService.PostDisciplineAsync($"{disciplineName}");
            if (result.Success)
            {
                return RedirectToAction("Index");
            }
            return BadRequest();
        }


    }
}
