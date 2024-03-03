using University.WebApi.Dtos;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using University.Web.Services.Contracts;
using Models.Models;
using University.WebApi.Dtos.PersonDtos;
using Microsoft.AspNetCore.Mvc;
using University.WebApi.Dtos.MethodologicalPublicationDto;
using University.WebApi.Dtos.ScientificPublicationDto;
using University.WebApi.Dtos.WorkPlanDtos;

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




        // Publications Section
        public async Task<Result<string>> AddPublicationAsync(UploadMethodologicalPublicationDto publicationDto)
        {
            var client = GetClient();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(publicationDto), Encoding.UTF8, "application/json");

            var result = await client.PostAsync("/api/Publications/upload", jsonContent);
            if (!result.IsSuccessStatusCode)
                return Result<string>.Fail($"Failed to upload publication. Status code: {result.StatusCode}");

            var resultString = await result.Content.ReadAsStringAsync();
            return Result<string>.Ok(resultString);
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

        public async Task<Result<string>> EditPublication(MethodologicalPublicationDto model, int id)
        {
            var client = GetClient();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var result = await client.PostAsync($"/api/Publications/modify/{id}", jsonContent);

            if (!result.IsSuccessStatusCode)
                return Result<string>.Fail($"Failed to get department id. Status code: {result.StatusCode}");

            var resultString = await result.Content.ReadAsStringAsync();
            return Result<string>.Ok(resultString);
        }

        public async Task<List<Publication>?> GetDepartmentalPublicationsAsync()
        {
            HttpClient client = GetClient();
            string url = "api/publications/departmentalPublications";

            var result = await client.GetAsync(url);
            if(result.IsSuccessStatusCode)
            {
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };
                string response = await result.Content.ReadAsStringAsync();
                var publications = JsonConvert.DeserializeObject<List<Publication>>(response, settings);
                return publications;
            }
            return null;
        }



        // Person Section
        public async Task<Result<string>> AddPersonAsync(PostPersonDto postPersonDto)
        {
            var client = _httpClientService.Client;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Bearer);

            var jsonContent = new StringContent(JsonConvert.SerializeObject(postPersonDto), Encoding.UTF8, "application/json");

            var result = await client.PostAsync("/api/People", jsonContent);
            if (!result.IsSuccessStatusCode)
                return Result<string>.Fail($"Failed to add author. Status code: {result.StatusCode}");

            var stringContent = await result.Content.ReadAsStringAsync();
            return Result<string>.Ok(stringContent);
        }

        public async Task<List<GetPersonDto>?> GetPeopleAsync()
        {
            var client = _httpClientService.Client;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Bearer);

            HttpResponseMessage response = await client.GetAsync("/api/People");

            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                var persons = JsonConvert.DeserializeObject<List<GetPersonDto>>(resultString);
                if(persons != null) return persons;
            }

            return null;
        }



        // Lecturers Section
        public async Task<Result<List<LecturerDto>>> GetLecturersAsync()
        {
            var client = GetClient();

            var result = await client.GetAsync("/api/Lecturers/get-all");

            if (result.IsSuccessStatusCode)
            {
                var jsonContent = await result.Content.ReadAsStringAsync();
                var authors = JsonConvert.DeserializeObject<List<LecturerDto>>(jsonContent);

                if (authors != null)
                    return Result<List<LecturerDto>>.Ok(authors);
                else
                    return Result<List<LecturerDto>>.Fail("Failed to deserialize authors");
            }

            return Result<List<LecturerDto>>.Fail($"Failed to retrieve authors. Status code: {result.StatusCode}"); 
        }               
        public async Task<Result<Lecturer>> GetLecturerAsync(int id)
        {
            var client = GetClient();
            var result = await client.GetAsync($"/api/lecturers/get/{id}");
            
            if (result.IsSuccessStatusCode)
            {
                var jsonContent = await result.Content.ReadAsStringAsync();
                var author = JsonConvert.DeserializeObject<Lecturer>(jsonContent);

                if (author != null)
                    return Result<Lecturer>.Ok(author);
                else
                    return Result<Lecturer>.Fail("Failed to deserialize author");
            }

            return Result<Lecturer>.Fail($"Failed to retrieve lecturers. Status code: {result.StatusCode}"); 
        }





        // Methodology Publication Plans Section
        public async Task<Result<List<Plan>>> GetPlansAsync()
        {
            var client = GetClient();
            var result = await client.GetAsync($"/api/plans/get");

            if (result.IsSuccessStatusCode)
            {
                var jsonContent = await result.Content.ReadAsStringAsync();
                var plans = JsonConvert.DeserializeObject<List<Plan>>(jsonContent);

                if (plans != null)
                    return Result<List<Plan>>.Ok(plans);
                else
                    return Result<List<Plan>>.Fail("Failed to deserialize plans");
            }

            return Result<List<Plan>>.Fail($"Failed to retrieve plans. Status code: {result.StatusCode}");
        }

        public async Task<Result<byte[]>> DownloadPlanAsync(int id)
        {
            var client = GetClient();
            var result = await client.GetAsync($"/api/plans/get-pdf/{id}");

            if (result.IsSuccessStatusCode)
            {
                var jsonContent = await result.Content.ReadAsStringAsync();
                var plan = JsonConvert.DeserializeObject<byte[]>(jsonContent);

                if (plan != null)
                    return Result<byte[]>.Ok(plan);
                else
                    return Result<byte[]>.Fail("Failed to deserialize plan");
            }

            return Result<byte[]>.Fail($"Failed to retrieve plan. Status code: {result.StatusCode}");
        }

        public async Task<Result<string>> AddPlanAsync(AddPlanDto addPlanDto)
        {
            var client = GetClient();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(addPlanDto), Encoding.UTF8, "application/json");

            var result = await client.PostAsync("/api/plans/add", jsonContent);
            if (!result.IsSuccessStatusCode)
                return Result<string>.Fail($"Failed to upload publication. Status code: {result.StatusCode}");

            var resultString = await result.Content.ReadAsStringAsync();
            return Result<string>.Ok(resultString);
        }        

        public async Task<Result<Plan>> GetPlanInfoAsync(int id)
        {
            var client = GetClient();
            var result = await client.GetAsync($"/api/plans/get");

            if (result.IsSuccessStatusCode)
            {
                var jsonContent = await result.Content.ReadAsStringAsync();
                var plans = JsonConvert.DeserializeObject<List<Plan>>(jsonContent);

                if (plans != null)
                    return Result<Plan>.Ok(plans.First(p => p.PlanId == id));
                else
                    return Result<Plan>.Fail("Failed to deserialize plans");
            }

            return Result<Plan>.Fail($"Failed to retrieve plans. Status code: {result.StatusCode}");
        }

        
        // Scientific Publications Section
        public async Task PostScientificPublicationAsync(PostScientificPublicationDto publicationDto)
        {
            var client = GetClient();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(publicationDto), Encoding.UTF8, "application/json");

            var result = await client.PostAsync("api/ScientificPublications", jsonContent);
        }



        // Departments Section
        public async Task<Result<List<Department>>> GetDepartmentsAsync()
        {
            var client = GetClient();
            var result = await client.GetAsync($"/api/departments");

            if (result.IsSuccessStatusCode)
            {
                var jsonContent = await result.Content.ReadAsStringAsync();
                var departments = JsonConvert.DeserializeObject<List<Department>>(jsonContent);

                if (departments != null)
                    return Result<List<Department>>.Ok(departments);
                else
                    return Result<List<Department>>.Fail("Failed to deserialize departments");
            }

            return Result<List<Department>>.Fail($"Failed to retrieve departments. Status code: {result.StatusCode}");
        }
        public async Task<Result<int>> GetCurrentDepartmentId()
        {
            var client = GetClient();

            var result = await client.GetAsync("/api/auth/get-department-id");
            if (!result.IsSuccessStatusCode)
                return Result<int>.Fail($"Failed to get department id. Status code: {result.StatusCode}");

            var resultString = await result.Content.ReadAsStringAsync();
            return Result<int>.Ok(int.Parse(resultString));
        }



        // Disciplines Section
        public async Task<List<Discipline>?> GetDisciplinesListAsync()
        {
            var client = GetClient();
            var result = await client.GetAsync(@"/api/Disciplines/list");

            var jsonContent = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Discipline>>(jsonContent);
        }
        public async Task<Result<List<Discipline>>> GetDisciplinesAsync()
        {
            var client = GetClient();
            var result = await client.GetAsync(@"/api/Disciplines");

            if(result.IsSuccessStatusCode)
            {
                var jsonContent = await result.Content.ReadAsStringAsync();
                var disciplines = JsonConvert.DeserializeObject<List<Discipline>>(jsonContent);

                if (disciplines != null)
                    return Result<List<Discipline>>.Ok(disciplines);
                else
                    return Result<List<Discipline>>.Fail("Failed to deserialize disciplines");
            }
            return Result<List<Discipline>>.Fail($"Failed to retrieve disciplines. Status code: {result.StatusCode}");
        }

        public async Task<Result<Discipline>> GetDisciplineAsync(int id)
        {
            var client = GetClient();
            var result = await client.GetAsync(@$"/api/Disciplines/{id}");

            if (result.IsSuccessStatusCode)
            {
                var jsonContent = await result.Content.ReadAsStringAsync();
                var discipline = JsonConvert.DeserializeObject<Discipline>(jsonContent);

                if (discipline != null)
                    return Result<Discipline>.Ok(discipline);
                else
                    return Result<Discipline>.Fail("Failed to deserialize discipline");
            }
            return Result<Discipline>.Fail($"Failed to retrieve discipline. Status code: {result.StatusCode}");
        }

        public async Task<Result<string>> PostDisciplineAsync(string disciplineName)
        {
            var client = GetClient();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(disciplineName), Encoding.UTF8, "application/json");
            var result = await client.PostAsync(@$"/api/Disciplines", jsonContent);

            if (result.IsSuccessStatusCode)
            {
                return Result<string>.Ok("ok");
            }
            return Result<string>.Fail($"Failed to add discipline. Status code: {result.StatusCode}");
        }


        // Work plans section
        public async Task<List<GetWorkPlan>?> GetWorkPlansListAsync()
        {
            var client = GetClient();
            var response = await client.GetAsync("api/WorkPlans/guarantor");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var workPlans = JsonConvert.DeserializeObject<List<GetWorkPlan>>(content);
            return workPlans;
        }

        public async Task<List<Publication>?> GetLecturerCorrelation(int lectId, int discId)
        {
            var client = GetClient();
            var response = await client.GetAsync($"api/Lecturers/correlation?lectId={lectId}&discId={discId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

            var publications = JsonConvert.DeserializeObject<List<Publication>>(content, settings);
            return publications;
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
