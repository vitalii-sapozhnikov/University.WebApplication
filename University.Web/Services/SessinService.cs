using Models.Models;
using Newtonsoft.Json;
using University.Web.Services.Contracts;

namespace University.Web.Services
{
    public class SessionService : ISessionService
    {
        public bool isAuthorized { get => !string.IsNullOrEmpty(GetBearerToken()); }

        private const string BearerTokenSessionKey = "BearerToken";
        private const string CurrentUserSessionKey = "CurrentUser";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetBearerToken(string token)
        {
            _httpContextAccessor.HttpContext.Session.SetString(BearerTokenSessionKey, token);
        }

        public string? GetBearerToken()
        {
            return _httpContextAccessor.HttpContext.Session.GetString(BearerTokenSessionKey);
        }

        public void RemoveBearerToken()
        {
            _httpContextAccessor.HttpContext.Session.Remove(BearerTokenSessionKey);
        }

        public void SetUser(ApplicationUser userInfo)
        {
            _httpContextAccessor.HttpContext.Session.SetObject(CurrentUserSessionKey, userInfo);
        }

        public ApplicationUser GetUser()
        {
            return _httpContextAccessor.HttpContext.Session.GetObject<ApplicationUser>(CurrentUserSessionKey);
        }

        public void RemoveUser()
        {
            _httpContextAccessor.HttpContext.Session.RemoveObject(CurrentUserSessionKey);
        }
    }

    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
        public static void RemoveObject(this ISession session, string key)
        {
            session.Remove(key);
        }
    }

}
