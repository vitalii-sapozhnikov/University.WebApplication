using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using University.Web.Services.Contracts;

namespace University.Web.Services
{
    public class GoogleDriveStorageService : ICloudStorageService
    {
        private static readonly string credentialsPath = @"C:\Users\vital\Documents\Универ\4 курс\Диплом\University.WebApi\University.Web\client_secret_1025282072388-n9s9qjbmsa8m3008o824ee74d9lobu31.apps.googleusercontent.com.json";

        public async Task<string> UploadAsync(IFormFile file)
        {
            try
            {
                // Create Drive API service
                DriveService service = CreateDriveService(credentialsPath);

                // Upload the file to Google Drive
                var fileMetadata = new Google.Apis.Drive.v3.Data.File
                {
                    Name = Path.GetFileName(file.FileName),
                    MimeType = file.ContentType
                };

                FilesResource.CreateMediaUpload request;
                using (var stream = file.OpenReadStream())
                {
                    request = service.Files.Create(fileMetadata, stream, file.ContentType);
                    await request.UploadAsync();
                }

                var fileResponse = request.ResponseBody;

                // Return the file ID
                string fileId = fileResponse.Id;
                Console.WriteLine($"File uploaded with ID: {fileId}");

                return fileId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file to Google Drive: {ex.Message}");
                return null;
            }
        }

        public byte[] Download(string fileId)
        {
            // Create Drive API service
            DriveService service = CreateDriveService(credentialsPath);

            // Download the file content
            var request = service.Files.Get(fileId);
            using (var stream = new MemoryStream())
            {
                request.Download(stream);
                return stream.ToArray();
            }
        }


        private static DriveService CreateDriveService(string credentialsPath)
        {
            UserCredential credential;

            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { DriveService.Scope.DriveFile },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true),
                    new LocalServerCodeReceiver("http://localhost:63470")).Result;
            }

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "YourAppName",
            });

            return service;
        }
    }
}
