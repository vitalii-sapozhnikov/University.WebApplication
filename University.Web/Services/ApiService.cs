using Models.Dtos;
using Newtonsoft.Json;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Text;
using University.Web.Services.Contracts;
using University.WebApi.Models;

namespace University.Web.Services
{
    public class ApiService : IApiService
    {
        public string Bearer { get { return _authService.GetAccessToken(); } }
        private readonly IAuthService _authService;
        private readonly IHttpClientService _httpClientService;
        public ApiService(IAuthService authService, IHttpClientService httpClientService)
        {
            _authService = authService;
            _httpClientService = httpClientService;
        }

        public async Task<string> GetWeatherForecast()
        {
            var client = _httpClientService.Client;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Bearer);

            var result = await client.GetAsync("/WeatherForecast");
            var res = await result.Content.ReadAsStringAsync();
            return res;
        }

        public async Task<string> AddPublicationAsync(UploadPublicationDto publicationDto)
        {
            var client = _httpClientService.Client;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Bearer);

            var jsonContent = new StringContent(JsonConvert.SerializeObject(publicationDto), Encoding.UTF8, "application/json");

            var result = await client.PostAsync("/api/Publications/upload", jsonContent);
            var res = await result.Content.ReadAsStringAsync();
            return res;
        }

        public async Task<List<AuthorDto>> GetAuthorsAsync()
        {
            var client = _httpClientService.Client;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Bearer);

            var result = await client.GetAsync("/api/Authors/get-all");

            if (result.IsSuccessStatusCode)
            {
                var jsonContent = await result.Content.ReadAsStringAsync();
                var authors = JsonConvert.DeserializeObject<List<AuthorDto>>(jsonContent);
                return authors;
            }
            else
            {
                throw new HttpRequestException($"Failed to retrieve authors. Status code: {result.StatusCode}");
            }
        }

        public async Task<string> AddAuthorAsync(AddAuthorDto addAuthorDto)
        {
            var client = _httpClientService.Client;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Bearer);

            var jsonContent = new StringContent(JsonConvert.SerializeObject(addAuthorDto), Encoding.UTF8, "application/json");

            var result = await client.PostAsync("/api/Authors/add", jsonContent);
            var res = await result.Content.ReadAsStringAsync();
            return res;
        }
    }
}
