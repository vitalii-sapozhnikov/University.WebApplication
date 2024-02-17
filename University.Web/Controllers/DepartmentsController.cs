using Microsoft.AspNetCore.Mvc;
using University.Web.Services.Contracts;

namespace University.Web.Controllers
{
    [Route("[controller]")]
    public class DepartmentsController : Controller
    {
        private readonly IApiService _apiService;

        public DepartmentsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var resultAuthors = await _apiService.GetLecturersAsync();
            if (!resultAuthors.Success)
                return BadRequest(resultAuthors.ErrorMessage);

            var resultDepartments = await _apiService.GetDepartmentsAsync();
            if (!resultDepartments.Success)
                return BadRequest(resultDepartments.ErrorMessage);


            return View((resultAuthors.Data, resultDepartments.Data.Where(d => d.DepartmentId != 0).ToList()));
        }
    }
}
