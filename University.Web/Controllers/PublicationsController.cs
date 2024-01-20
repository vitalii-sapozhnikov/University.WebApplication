using Google.Apis.Drive.v3;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using Models.Dtos.WebApp;
using System.Text;
using University.Web.Services;
using University.Web.Services.Contracts;
using University.WebApi.Models;

namespace University.Web.Controllers
{
    public class PublicationsController : Controller
    {
        private readonly ICloudStorageService cloudStorageService;
        private readonly IApiService apiService;

        public PublicationsController(ICloudStorageService cloudStorageService, IApiService apiService)
        {
            this.cloudStorageService = cloudStorageService;
            this.apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upload() 
        {
            var model = new UploadPublication();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(UploadPublication model)
        {
            if (ModelState.IsValid)
            {
                #region File Upload
                string fileId = await cloudStorageService.UploadAsync(model.File);

                if (fileId == null)
                {
                    return BadRequest("Unable to upload file to GoogleDrive");
                }
                #endregion

                var publicationDto = new UploadPublicationDto
                {
                    Title = model.Title,
                    PublicationDate = model.PublicationDate ?? DateTime.Now,
                    CloudStorageId = fileId,
                    Abstract = model.Abstract,
                    Description = model.Description,
                    Keywords = model.Keywords.Split(',').Select(s => s.Trim().ToLower()).ToArray()
                };


                if (model.Authors != null)
                {
                    await AddAuthorsIfNotExist(model, publicationDto);
                }

                await apiService.AddPublicationAsync(publicationDto);

                return RedirectToAction("Index", "Home");

            }

            return View(model);

            async Task AddAuthorsIfNotExist(UploadPublication model, UploadPublicationDto publicationDto)
            {
                model.Authors = model.Authors.EndsWith(", ") ? model.Authors.Substring(0, model.Authors.Length - 2) : model.Authors;


                var authorsList = (await apiService.GetAuthorsAsync())
                                        .Select(a => $"{a.LastName} {a.FirstName} {a.MiddleName}").ToList();

                var publicationAuthors = model.Authors.Split(',').Select(a => a.Trim()).ToList();
                int length = publicationAuthors.Count();
                publicationDto.AuthorIds = new int[length];

                foreach (var pubAuthor in publicationAuthors)
                {
                    if (!authorsList.Contains(pubAuthor))
                    {
                        var parts = pubAuthor.Split(" ");

                        await apiService.AddAuthorAsync(new AddAuthorDto
                        {
                            LastName = parts[0],
                            FirstName = parts[1],
                            MiddleName = parts[2]
                        });
                    }
                }

                for (int i = 0; i < length; i++)
                {
                    var list = await apiService.GetAuthorsAsync();
                    var parts = publicationAuthors.ToList()[i].Split(" ");

                    publicationDto.AuthorIds[i] = list
                        .FirstOrDefault(a => a.LastName == parts[0] &&
                        a.FirstName == parts[1] && a.MiddleName == parts[2]).Id;
                }
            }
        }

        [Route("api/Authors/GetAuthors")]
        public async Task<IActionResult> GetAuthorsFromDatabase(string term)
        {
            // Your logic to fetch authors based on the search term
            var authors = (await apiService.GetAuthorsAsync())
                .Where(a => a.FirstName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                            a.LastName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                            a.MiddleName.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Select(a => $"{a.LastName} {a.FirstName} {a.MiddleName}")
                .ToList();

            return Ok(authors);
        }

        [HttpGet]
        public IActionResult Info(string fileId = "1OQ-ZrbaL389uHA_YwHrCGl9yC1vqFDPa") 
        {
            if (string.IsNullOrEmpty(fileId))
            {
                // Handle the case where fileId is not provided
                return RedirectToAction("Index"); // Redirect to the appropriate action
            }

            try
            {
                var file = cloudStorageService.Download(fileId);

                
                return File(file, "application/octet-stream");
            }
            catch (Exception ex)
            {
                // Handle the exception, log, or redirect to an error page
                return RedirectToAction("Index");
            }
        }

    }
}
