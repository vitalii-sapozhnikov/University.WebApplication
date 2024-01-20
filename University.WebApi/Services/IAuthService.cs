using Models.Dtos;
using Models.Models;

namespace University.WebApi.Services
{
    public interface IAuthService
    {
        string GenerateTokenString(LoginUser user);
        Task<ApplicationUser> GetUserInfo(LoginUser user);
        Task<(bool success, string? error)> Login(LoginUser user);
        Task<(bool success, string? error)> RegisterUser(RegisterUserDto user);
    }
}