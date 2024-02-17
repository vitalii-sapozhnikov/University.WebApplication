using University.WebApi.Dtos;

namespace University.Web.Services
{
    public interface IAuthService
    {
        string GetAccessToken();
        Task<(bool success, string? error)> Login(LoginUser user);
        void Logout();
    }
}