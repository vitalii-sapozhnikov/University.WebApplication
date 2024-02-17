using University.WebApi.Dtos;
using Models.Models;

namespace University.WebApi.Services
{
    public interface IAuthService
    {
        string GenerateTokenString(LoginUser user, string role);
        Task<IList<string>> GetRole(string email);
        Task<ApplicationUser> GetUser(string email);
        Task<ApplicationUser> GetUserInfo(LoginUser user);
        Task<(bool success, string? error)> Login(LoginUser user);
        Task<(bool success, string? error)> RegisterUser(RegisterUserDto user);
    }
}