using Microsoft.AspNetCore.Mvc;
using University.Web.Services.Contracts;

namespace University.Web.Controllers
{
    [Route("[controller]")]
    public class LicenseController : Controller
    {
        private readonly IApiService _apiService;

        public LicenseController(IApiService apiService)
        {
            _apiService = apiService;
        }

        
    }
}
