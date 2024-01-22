using Models.Dtos;
using Newtonsoft.Json;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Text;
using University.Web.Services.Contracts;
using University.WebApi.Models;
using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Build.Framework;

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
        private HttpClient GetClient()
        {
            var client = _httpClientService.Client;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Bearer);
            return client;
        }
        public async Task<Result<string>> AddPublicationAsync(UploadPublicationDto publicationDto)
        {
            var client = GetClient();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(publicationDto), Encoding.UTF8, "application/json");

            var result = await client.PostAsync("/api/Publications/upload", jsonContent);
            if (!result.IsSuccessStatusCode)
                return Result<string>.Fail($"Failed to upload publication. Status code: {result.StatusCode}");

            var resultString = await result.Content.ReadAsStringAsync();
            return Result<string>.Ok(resultString);
        }
        public async Task<Result<List<AuthorDto>>> GetAuthorsAsync()
        {
            var client = GetClient();

            var result = await client.GetAsync("/api/Authors/get-all");

            if (result.IsSuccessStatusCode)
            {
                var jsonContent = await result.Content.ReadAsStringAsync();
                var authors = JsonConvert.DeserializeObject<List<AuthorDto>>(jsonContent);

                if (authors != null)
                    return Result<List<AuthorDto>>.Ok(authors);
                else
                    return Result<List<AuthorDto>>.Fail("Failed to deserialize authors");
            }

            return Result<List<AuthorDto>>.Fail($"Failed to retrieve authors. Status code: {result.StatusCode}"); 
        }
        public async Task<Result<string>> AddAuthorAsync(AddAuthorDto addAuthorDto)
        {
            var client = _httpClientService.Client;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Bearer);

            var jsonContent = new StringContent(JsonConvert.SerializeObject(addAuthorDto), Encoding.UTF8, "application/json");

            var result = await client.PostAsync("/api/Authors/add", jsonContent);
            if (!result.IsSuccessStatusCode)
                return Result<string>.Fail($"Failed to add author. Status code: {result.StatusCode}");

            var stringContent = await result.Content.ReadAsStringAsync();
            return Result<string>.Ok(stringContent);
        }
        public async Task<Result<List<PublicationShortDto>>> GetPublicationsAsync(
            string? searchTerm = null,
            string? authorName = null,
            DateTime? startDateFilter = null,
            DateTime? endDateFilter = null)
        {
            var client = GetClient();

            // Build the URL with query parameters
            var url = "/api/Publications";

            var queryString = new StringBuilder("?");
            if (!string.IsNullOrEmpty(searchTerm))
            {
                queryString.Append($"searchTerm={Uri.EscapeDataString(searchTerm)}&");
            }

            if (!string.IsNullOrEmpty(authorName))
            {
                queryString.Append($"authorName={Uri.EscapeDataString(authorName)}&");
            }

            if (startDateFilter.HasValue)
            {
                queryString.Append($"startDateFilter={startDateFilter.Value.ToString("yyyy-MM-dd")}&");
            }

            if (endDateFilter.HasValue)
            {
                queryString.Append($"endDateFilter={endDateFilter.Value.ToString("yyyy-MM-dd")}&");
            }

            // Remove trailing "&" from the queryString
            if (queryString.Length > 1)
            {
                queryString.Length--; // Remove the last character
            }

            url += queryString.ToString();

            var result = await client.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var jsonContent = await result.Content.ReadAsStringAsync();
                var publications = JsonConvert.DeserializeObject<List<PublicationShortDto>>(jsonContent);

                if (publications != null)
                {
                    return Result<List<PublicationShortDto>>.Ok(publications);
                }
                else
                {
                    return Result<List<PublicationShortDto>>.Fail("Failed to deserialize publications");
                }
            }

            return Result<List<PublicationShortDto>>.Fail($"Failed to retrieve publications. Status code: {result.StatusCode}");
        }

        public async Task<Result<Publication>> GetPublicationInfoAsync(int idx)
        {
            var client = GetClient();

            var result = await client.GetAsync($"/api/Publications/info/{idx}");
            if (result.IsSuccessStatusCode)
            {
                var jsonContent = await result.Content.ReadAsStringAsync();
                var publication = JsonConvert.DeserializeObject<Publication>(jsonContent);

                if (publication != null)
                    return Result<Publication>.Ok(publication);
                else
                    return Result<Publication>.Fail("Failed to deserialize publication");
            }
            return Result<Publication>.Fail($"Failed to retrieve publication. Status code: {result.StatusCode}");

        }
    }

    public class Result<T>
    {
        public bool Success { get; }
        public T Data { get; }
        public string ErrorMessage { get; }

        private Result(bool success, T data, string errorMessage)
        {
            Success = success;
            Data = data;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Ok(T data) => new Result<T>(true, data, null);
        public static Result<T> Fail(string errorMessage) => new Result<T>(false, default, errorMessage);
    }

}
