using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using University.WebApi.Dtos;
using Models.Roles;
using University.WebApi.Contexts;
using University.WebApi.Services;
using Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Newtonsoft.Json;

namespace University.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private AppDbContext _appDbContext;
        private UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthService authService, AppDbContext appDbContext, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _appDbContext = appDbContext;
            _userManager = userManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(RegisterUserDto user)
        {
            var (success, error) = await _authService.RegisterUser(user);
                      

            //if (success)
            //{
            //    switch (user.Role)
            //    {
            //        case Roles.Lecturer:
            //            Lecturer lecturer = new Lecturer
            //            {
            //                FirstName = user.FirstName,
            //                LastName = user.LastName,
            //                MiddleName = user.MiddleName,
            //                ApplicationUser = await _authService.GetUser(user.Email),
            //                Departments = { _appDbContext.Departments.First(d => d.DepartmentId == user.DepartmentId) }
            //            };

            //            await _appDbContext.Lecturers.AddAsync(lecturer);
            //            await _appDbContext.SaveChangesAsync();
            //            break;

            //        case Roles.HeadOfDepartment:
            //            HeadOfDepartment headOfDepartment = new HeadOfDepartment
            //            {
            //                FirstName = user.FirstName,
            //                LastName = user.LastName,
            //                MiddleName = user.MiddleName,
            //                ApplicationUser = await _authService.GetUser(user.Email),
            //                DepartmentId = user.DepartmentId,
            //            }; 
                        
            //            await _appDbContext.HeadsOfDepartments.AddAsync(headOfDepartment);
            //            await _appDbContext.SaveChangesAsync();
            //            break;
            //    }

            //}
                return Ok("Successfully done");

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
                var roles = await _authService.GetRole(user.Email);
                var tokenString = _authService.GenerateTokenString(user, roles.First());

                var userInfo = await _authService.GetUserInfo(user);

                var response = new
                {
                    Token = tokenString,
                    UserInfo = userInfo,
                    UserRole = roles.First()
                };

                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                return Ok(JsonConvert.SerializeObject(response, Formatting.Indented, settings));
            }

            // Return a more detailed error message
            return BadRequest($"Login failed. {error}");
        }

        [Authorize]
        [HttpGet("get-department-id")]
        public async Task<IActionResult> GetMyDepartmentId()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null) { return NotFound("User not found!"); }
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null) { return NotFound("User not found!"); }

            int departmentId = 0;

            switch((await _userManager.GetRolesAsync(user)).First())
            {
                case Roles.Lecturer:
                    departmentId = _appDbContext.Lecturers.First(l => l.ApplicationUser == user).Departments.First().DepartmentId;
                    break;
                case Roles.HeadOfDepartment:
                    departmentId = _appDbContext.HeadsOfDepartments.First(h => h.ApplicationUser == user).DepartmentId;
                    break;
            }

            return Ok(departmentId);
        }

    }
}
