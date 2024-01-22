using Google.Apis.Drive.v3;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using System.Net.Mime;
using System.Text;
using University.Web.Models.Post;
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

        [HttpGet]
        [Route("publications/all")]
        public async Task<IActionResult> All(
            string searchTerm,
            string authorName,
            DateTime? startDateFilter,
            DateTime? endDateFilter)
        {
            var result = await apiService.GetPublicationsAsync(
                searchTerm,
                authorName,
                startDateFilter,
                endDateFilter);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            ViewBag.SearchTerm = searchTerm;
            ViewBag.AuthorName = authorName;
            ViewBag.StartDateFilter = startDateFilter;
            ViewBag.EndDateFilter = endDateFilter;

            return View("Index", result.Data);
        }


        [HttpGet]
        [Route("publications/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            var result = await apiService.GetPublicationInfoAsync(id);
            if(!result.Success)
                return BadRequest(result.ErrorMessage);

            return View("IndexId", result.Data);
        }

        [HttpGet]
        [Route("publications/upload")]
        public IActionResult Upload() 
        {
            var model = new UploadPublication();
            return View(model);
        }

        [HttpGet]
        public IActionResult DownloadFile(string id)
        {
            var fileBytes = cloudStorageService.Download(id);

            string contentType = "application/pdf";

            var contentDisposition = new ContentDisposition
            {
                Inline = true
            };

            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

            return File(fileBytes, contentType);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(UploadPublication model)
        {
            if (!ModelState.IsValid)
                return View(model);

            #region File Upload
            string fileId = await cloudStorageService.UploadAsync(model.File);

            if (fileId == null)
                return BadRequest("Unable to upload file to GoogleDrive");
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
                model.Authors = model.Authors.EndsWith(", ") ? model.Authors.Substring(0, model.Authors.Length - 2) : model.Authors;

                var result = await apiService.GetAuthorsAsync();
                if (!result.Success)
                {
                    return BadRequest(result.ErrorMessage);
                }

                var authorsList = result.Data.Select(a => $"{a.LastName} {a.FirstName} {a.MiddleName}").ToList();

                var publicationAuthors = model.Authors.Split(',').Select(a => a.Trim()).ToList();
                int length = publicationAuthors.Count();
                publicationDto.AuthorIds = new int[length];

                foreach (var pubAuthor in publicationAuthors)
                {
                    if (!authorsList.Contains(pubAuthor))
                    {
                        var parts = pubAuthor.Split(" ");

                        var r = await apiService.AddAuthorAsync(new AddAuthorDto
                        {
                            LastName = parts[0],
                            FirstName = parts[1],
                            MiddleName = parts[2]
                        });

                        if (!r.Success) return BadRequest(r.ErrorMessage);
                    }
                }

                result = await apiService.GetAuthorsAsync();
                if (!result.Success)
                    return BadRequest(result.ErrorMessage);

                for (int i = 0; i < length; i++)
                {
                    var parts = publicationAuthors.ToList()[i].Split(" ");

                    var matchingAuthor = result.Data.FirstOrDefault(a =>
                        a.LastName == parts[0] &&
                        a.FirstName == parts[1] &&
                        a.MiddleName == parts[2]);

                    if (matchingAuthor != null)
                        publicationDto.AuthorIds[i] = matchingAuthor.Id;
                    else
                        return NotFound("Author not found");
                }
            }

            var apiResult = await apiService.AddPublicationAsync(publicationDto);
            if (!apiResult.Success)
                return BadRequest(apiResult.ErrorMessage);

            return RedirectToAction("Index", "Home");
        }

        [Route("api/Authors/GetAuthors")]
        public async Task<IActionResult> GetAuthorsFromDatabase(string term)
        {
            var result = await apiService.GetAuthorsAsync();
            if(!result.Success)
                return BadRequest(result?.ErrorMessage);

            var authors = result.Data
                .Where(a => a.FirstName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                            a.LastName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                            a.MiddleName.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Select(a => $"{a.LastName} {a.FirstName} {a.MiddleName}")
                .ToList();

            return Ok(authors);
        }


        //[HttpGet]
        //public IActionResult Info(string fileId = "1OQ-ZrbaL389uHA_YwHrCGl9yC1vqFDPa") 
        //{
        //    if (string.IsNullOrEmpty(fileId))
        //    {
        //        // Handle the case where fileId is not provided
        //        return RedirectToAction("Index"); // Redirect to the appropriate action
        //    }

        //    try
        //    {
        //        var file = cloudStorageService.Download(fileId);

                
        //        return File(file, "application/octet-stream");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle the exception, log, or redirect to an error page
        //        return RedirectToAction("Index");
        //    }
        //}

    }
}
