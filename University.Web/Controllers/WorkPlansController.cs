using Microsoft.AspNetCore.Mvc;
using Models.Roles;
using University.Web.Services;
using University.Web.Services.Contracts;

namespace University.Web.Controllers
{
    [Route("[controller]")]
    public class WorkPlansController : Controller
    {
        private readonly IApiService _apiService;

        public WorkPlansController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [AuthorizeSession(Roles.GuarantorOfSpeciality)]
        public async Task<IActionResult> Index()
        {
            var workPlans = await _apiService.GetWorkPlansListAsync();
            return View(workPlans);
        }
    }
}
