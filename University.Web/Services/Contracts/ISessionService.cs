using Models.Models;

namespace University.Web.Services.Contracts
{
    public interface ISessionService
    {
        bool isAuthorized { get; }

        string GetBearerToken();
        (ApplicationUser user, string role) GetUser();
        void RemoveBearerToken();
        void RemoveUser();
        void SetBearerToken(string token);
        void SetUser(ApplicationUser userInfo, string role);
    }
}