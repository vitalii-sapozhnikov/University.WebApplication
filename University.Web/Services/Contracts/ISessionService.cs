using Models.Models;

namespace University.Web.Services.Contracts
{
    public interface ISessionService
    {
        bool isAuthorized { get; }

        string GetBearerToken();
        ApplicationUser GetUser();
        void RemoveBearerToken();
        void RemoveUser();
        void SetBearerToken(string token);
        void SetUser(ApplicationUser userInfo);
    }
}