using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using University.Web.Services;
using University.Web.Services.Contracts;

namespace University.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IApiService _apiService;

        public AccountController(IAuthService authService, IApiService apiService)
        {
            _authService = authService;
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginUser();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUser model)
        {
            if (ModelState.IsValid)
            {
                var (result, error) = await _authService.Login(model);

                if (result)
                {
                    // Store the token in session, cookie, etc.
                    // Redirect to the desired page
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            _authService.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterUserDto userDto = new RegisterUserDto();
            return View(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto model)
        {
            if (ModelState.IsValid)
            {

            }
            return View(model);
        }
    }
}
