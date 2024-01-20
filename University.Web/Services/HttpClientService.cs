using System.Net.Http.Headers;
using University.Web.Services.Contracts;

namespace University.Web.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;

        public HttpClient Client { get { return _httpClient; } }

        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
