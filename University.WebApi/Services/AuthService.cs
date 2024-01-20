using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Models.Dtos;
using Models.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace University.WebApi.Services
{
    public class AuthService : IAuthService
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
            _roleManager = roleManager;
        }

       

        public async Task<(bool success, string? error)> RegisterUser(RegisterUserDto user)
        {
            var identityUser = new ApplicationUser
            {
                UserName = user.Email,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName
            };

            var result = await _userManager.CreateAsync(identityUser, user.Password);

            if (result.Succeeded)
            {
                // User successfully created, now add roles
                var res = await _userManager.AddToRoleAsync(identityUser, user.Role);

                if (res.Succeeded)
                {
                    return (true, null); // Success
                }
                else
                {
                    // If adding roles failed
                    var errorMessage = string.Join(", ", res.Errors.Select(e => e.Description));
                    return (false, errorMessage);
                }
            }
            else
            {
                // If user creation failed
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, errorMessage);
            }
        }

        public async Task<(bool success, string? error)> Login(LoginUser user)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.Email);
            if (identityUser is null)
            {
                return (false, "User not found");
            }

            var signInResult = await _userManager.CheckPasswordAsync(identityUser, user.Password);

            if (signInResult)
            {
                return (true, null); // Success
            }
            else
            {
                // If login failed, concatenate the error messages for a more detailed response
                var errorMessage = "Invalid login attempt.";
                return (false, errorMessage);
            }
        }

        public string GenerateTokenString(LoginUser user)
        {
            IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "User"),

            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
            SigningCredentials signingCredential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(180),
                issuer: _config.GetSection("Jwt:Issuer").Value,
                audience: _config.GetSection("Jwt:Audience").Value,
                signingCredentials: signingCredential);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }

        public async Task<ApplicationUser> GetUserInfo(LoginUser user)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.Email);
            if (identityUser is null)
            {
                return (null);
            }

            var signInResult = await _userManager.CheckPasswordAsync(identityUser, user.Password);

            if (signInResult)
            {
                return (identityUser); // Success
            }

            return null;
        }
    }
}
