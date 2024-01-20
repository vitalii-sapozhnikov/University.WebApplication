using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using University.WebApi.Services;

namespace University.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(RegisterUserDto user)
        {
            var (success, error) = await _authService.RegisterUser(user);

            if (success)
            {
                return Ok("Successfully done");
            }

            // Return a more detailed error message
            return BadRequest($"Registration failed. Error: {error}");
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, error) = await _authService.Login(user);

            if (success)
            {
                var tokenString = _authService.GenerateTokenString(user);

                var userInfo = await _authService.GetUserInfo(user);

                var response = new
                {
                    Token = tokenString,
                    UserInfo = userInfo
                };

                return Ok(response);
            }

            // Return a more detailed error message
            return BadRequest($"Login failed. {error}");
        }


    }
}
