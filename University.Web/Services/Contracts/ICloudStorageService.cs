
namespace University.Web.Services.Contracts
{
    public interface ICloudStorageService
    {
        byte[] Download(string fileId);
        Task<string> UploadAsync(IFormFile file);
    }
}