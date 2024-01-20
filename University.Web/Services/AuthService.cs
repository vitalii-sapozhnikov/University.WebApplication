using Models.Dtos;
using NuGet.Protocol;
using System.Text;
using University.Web.Services.Contracts;
using Newtonsoft.Json.Linq;
using Models.Models;

namespace University.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ICookieService _cookieService;
        private readonly ISessionService _sessionService;

        public AuthService(IHttpClientService httpClientService, ICookieService cookieService, ISessionService sessionService)
        {
            _httpClientService = httpClientService;
            _cookieService = cookieService;
            _sessionService = sessionService;
        }

        public async Task<(bool success, string? error)> Login(LoginUser user)
        {
            var jsonPayload = user.ToJson();

            // Send POST request
            var response = await _httpClientService.Client.PostAsync("/api/Auth/Login", new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

            // Handle the response as needed
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseContent);

                var token = jsonResponse["token"]?.ToString();
                var userInfo = jsonResponse["userInfo"]?.ToObject<ApplicationUser>();

                if (!string.IsNullOrEmpty(token) && userInfo != null)
                {
                    _sessionService.SetBearerToken(token);

                    _sessionService.SetUser(userInfo);

                    return (true, null);
                }

                return (true, null);
            }

            return (false, "Error");
        }

        public string GetAccessToken() 
        {
            //return _cookieService.GetBearerToken();
            return _sessionService.GetBearerToken();
        }
    }
}
