using Models.Dtos;
using University.WebApi.Models;

namespace University.Web.Services.Contracts
{
    public interface IApiService
    {
        string Bearer { get; }

        Task<Result<string>> AddAuthorAsync(AddAuthorDto addAuthorDto);
        Task<Result<string>> AddPublicationAsync(UploadPublicationDto publicationDto);
        Task<Result<List<AuthorDto>>> GetAuthorsAsync();
        Task<Result<Publication>> GetPublicationInfoAsync(int idx);
        Task<Result<List<PublicationShortDto>>> GetPublicationsAsync(string? searchTerm = null, string? authorName = null, DateTime? startDateFilter = null, DateTime? endDateFilter = null);
    }
}