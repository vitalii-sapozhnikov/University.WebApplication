using Models.Dtos;

namespace University.Web.Services.Contracts
{
    public interface IApiService
    {
        string Bearer { get; }

        Task<string> AddAuthorAsync(AddAuthorDto addAuthorDto);
        Task<string> AddPublicationAsync(UploadPublicationDto publicationDto);
        Task<List<AuthorDto>> GetAuthorsAsync();
        Task<string> GetWeatherForecast();
    }
}