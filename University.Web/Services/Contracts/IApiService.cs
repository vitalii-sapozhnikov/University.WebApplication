using University.WebApi.Dtos;
using Models.Models;
using University.WebApi.Dtos.PersonDtos;
using Microsoft.AspNetCore.Mvc;
using University.WebApi.Dtos.MethodologicalPublicationDto;
using University.WebApi.Dtos.ScientificPublicationDto;
using University.WebApi.Dtos.WorkPlanDtos;
using Models.Models.AdditionalTypes;

namespace University.Web.Services.Contracts
{
    public interface IApiService
    {
        string Bearer { get; }

        Task<Result<string>> AddPersonAsync(PostPersonDto addAuthorDto);
        Task<Result<string>> AddPublicationAsync(UploadMethodologicalPublicationDto publicationDto);
        Task<Result<List<LecturerDto>>> GetLecturersAsync();
        Task<Result<Publication>> GetPublicationInfoAsync(int idx);
        Task<Result<List<PublicationShortDto>>> GetPublicationsAsync(string? searchTerm = null, string? authorName = null, DateTime? startDateFilter = null, DateTime? endDateFilter = null, int[]? categories = null);
        Task<Result<Lecturer>> GetLecturerAsync(int id);
        Task<Result<List<Plan>>> GetPlansAsync();
        Task<Result<byte[]>> DownloadPlanAsync(int id);
        Task<Result<string>> AddPlanAsync(AddPlanDto addPlanDto);
        Task<Result<int>> GetCurrentDepartmentId();
        Task<Result<Plan>> GetPlanInfoAsync(int id);
        Task<Result<string>> EditPublication(MethodologicalPublicationDto model, int id);
        Task<Result<List<Department>>> GetDepartmentsAsync();
        Task<Result<List<Discipline>>> GetDisciplinesAsync();
        Task<Result<Discipline>> GetDisciplineAsync(int id);
        Task<Result<string>> PostDisciplineAsync(string disciplineName);
        Task<List<GetPersonDto>?> GetPeopleAsync();
        Task<List<Discipline>?> GetDisciplinesListAsync();
        Task PostScientificPublicationAsync(PostScientificArticleDto publicationDto);
        Task<List<Publication>?> GetDepartmentalPublicationsAsync();
        Task<List<GetWorkPlan>?> GetWorkPlansListAsync();
        Task<List<Publication>?> GetLecturerCorrelation(int lectId, int discId);
        Task PostScientificMonographAsync(PostScientificMonographDto publicationDto);
        Task PostScientificDissertationAsync(PostScientificDissertationDto publicationDto);
        Task PostScientificPatentAsync(PostScientificPatentDto publicationDto);
        Task PostScientificConferenceThesesAsync(PostScientificConferenceThesesDto publicationDto);
        Task<LecturerLicense> GetLecturerLicenseInfo(int lecturerId);
        Task DeletePublication(int id);
    }
}