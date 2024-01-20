using University.Web.Services.Contracts;

namespace University.Web.Services
{
    public class CookieService : ICookieService
    {
        private const string BearerTokenCookieName = "BearerToken";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetBearerToken(string token)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = _httpContextAccessor.HttpContext.Request.IsHttps
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append(BearerTokenCookieName, token, options);
        }

        public string GetBearerToken()
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[BearerTokenCookieName];
        }

        public void RemoveBearerToken()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(BearerTokenCookieName);
        }
    }
}
