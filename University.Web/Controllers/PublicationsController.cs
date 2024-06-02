using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using University.Web.Models.Post;
using University.Web.Services;
using University.Web.Services.Contracts;
using Models.Models;
using University.WebApi.Dtos.PersonDtos;
using Models.Roles;
using University.WebApi.Dtos.MethodologicalPublicationDto;
using University.WebApi.Dtos.ScientificPublicationDto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace University.Web.Controllers
{
    [Route("[controller]")]
    public class PublicationsController : Controller
    {
        private readonly ICloudStorageService cloudStorageService;
        private readonly IApiService apiService;
        private readonly ISessionService sessionService;

        public PublicationsController(ICloudStorageService cloudStorageService, IApiService apiService, ISessionService sessionService)
        {
            this.cloudStorageService = cloudStorageService;
            this.apiService = apiService;
            this.sessionService = sessionService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> All(
            string searchTerm,
            string authorName,
            DateTime? startDateFilter,
            DateTime? endDateFilter, int[]? categories = null)
        {
            var result = await apiService.GetPublicationsAsync(
                searchTerm,
                authorName,
                startDateFilter,
                endDateFilter,
                categories);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }   

            var disciplines = (await apiService.GetDisciplinesListAsync());
            var categoriesList = new SelectList(disciplines, "Id", "Name", categories);
            if(categories != null)
            {
                foreach (var item in categoriesList)
                {
                    if (categories.Contains(int.Parse(item.Value))) item.Selected = true;
                }
            }

            ViewBag.SearchTerm = searchTerm;
            ViewBag.AuthorName = authorName;
            ViewBag.StartDateFilter = startDateFilter;
            ViewBag.EndDateFilter = endDateFilter;
            ViewBag.Categories = categoriesList;

            return View("Index", result.Data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id)
        {
            var result = await apiService.GetPublicationInfoAsync(id);
            if(!result.Success)
                return BadRequest(result.ErrorMessage);



            return View("IndexId", result.Data);
        }


        [AuthorizeSession(Roles.Lecturer, Roles.HeadOfDepartment)]
        [HttpGet("upload")]
        public async Task<IActionResult> UploadPublication() 
        {
            var disciplines = (await apiService.GetDisciplinesListAsync());
            var authors = await apiService.GetPeopleAsync();
            var model = new UploadPublication();
            model.PublicationDate = DateTime.Now;
            ViewBag.Disciplines = disciplines;
            ViewBag.Authors = authors;

            return View("Upload", model);
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

        
        [HttpPost("upload-web")]
        public async Task<IActionResult> Upload(UploadPublication model)
        {
            if (!ModelState.IsValid)
            {
                var disciplines = (await apiService.GetDisciplinesListAsync());
                var authors = await apiService.GetPeopleAsync();

                ViewBag.Disciplines = disciplines;
                ViewBag.Authors = authors;

                return View(model);
            }
            if (model.PublicationType == PublicationType.WorkProgramme && model.DisciplinesIds.Count() > 1)
            {
                ModelState.AddModelError("DisciplinesIds", "You can only select one discipline for a Work Programme publication.");
                var disciplines = (await apiService.GetDisciplinesListAsync());
                var authors = await apiService.GetPeopleAsync();

                ViewBag.Disciplines = disciplines;
                ViewBag.Authors = authors;
                return View(model);            
            }

            #region File Upload
            string fileId = await cloudStorageService.UploadAsync(model.File);

            if (fileId == null)
                return BadRequest("Unable to upload file to GoogleDrive");
            #endregion

            var publicationDto = new UploadMethodologicalPublicationDto
            {
                Title = model.Title,
                PublicationDate = model.PublicationDate ?? DateTime.Now,
                CloudStorageId = fileId,
                Abstract = model.Abstract,
                Description = model.Description,
                PublicationType = model.PublicationType,
                DisciplineIds = model.DisciplinesIds,
                Keywords = model.Keywords.Split(',').Select(s => s.Trim().ToLower()).ToArray(),
                Volume = model.Volume,
                Language = model.Language
            };

            if (model.Authors != null)
            {                    
                model.Authors = model.Authors.EndsWith(", ") ? model.Authors.Substring(0, model.Authors.Length - 2) : model.Authors;

                var result = await apiService.GetLecturersAsync();
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

                        var r = await apiService.AddPersonAsync(new PostPersonDto
                        {
                            LastName = parts[0],
                            FirstName = parts[1],
                            MiddleName = parts[2]
                        });

                        if (!r.Success) return BadRequest(r.ErrorMessage);
                    }
                }

                result = await apiService.GetLecturersAsync();
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


        [AuthorizeSession(Roles.Lecturer, Roles.HeadOfDepartment)]
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await apiService.GetPublicationInfoAsync(id);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            Publication publication = result.Data;

            if(publication is MethodologicalPublication methPub) 
            {
                EditPublication model = new EditPublication
                {
                    Id = methPub.PublicationId,
                    Title = methPub.Title,
                    Abstract = methPub.Abstract,
                    Description = methPub.Description,
                    Volume = methPub.Volume,
                    Language = methPub.Language,
                    Type = methPub.Type
                };

                if(methPub.Keywords != null)
                {
                    model.Keywords = string.Join(", ", methPub.Keywords);
                }

                return View(model);
            }

            return View();
        }

        [AuthorizeSession(Roles.Lecturer, Roles.HeadOfDepartment)]
        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(EditPublication model, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var editPublicatioin = new MethodologicalPublicationDto
            {
                Title = model.Title,
                Abstract = model.Abstract,
                Description = model.Description,
                Keywords = model.Keywords.Split(',').Select(s => s.Trim().ToLower()).ToArray(),
                Volume = model.Volume,
                isPublished = true,
                Language = model.Language,
                Type = model.Type,
                PublicationDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
            };



            if (model.File != null)
            {
                string fileId = await cloudStorageService.UploadAsync(model.File);

                if (fileId == null)
                    return BadRequest("Unable to upload file to GoogleDrive");

                editPublicatioin.CloudStorageGuid = fileId;
            }
            await apiService.EditPublication(editPublicatioin, id);


            return RedirectToAction("All");
        }


        [AuthorizeSession(Roles.Lecturer, Roles.HeadOfDepartment)]
        [HttpGet("upload-scientific")]
        public async Task<IActionResult> UploadScientificPublication()
        {
            var disciplines = (await apiService.GetDisciplinesListAsync());
            var authors = await apiService.GetPeopleAsync();
            var publication = new PostScientificArticleDto();
            publication.PublicationDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            ViewBag.Disciplines = disciplines;
            ViewBag.Authors = authors;

            return View(publication);
        }

        [AuthorizeSession(Roles.Lecturer, Roles.HeadOfDepartment)]
        [HttpPost("upload-scientific")]
        public async Task<IActionResult> UploadScientificPublication(PostScientificArticleDto publicationDto)
        {
            if (!ModelState.IsValid)
                return View(publicationDto);

            await apiService.PostScientificPublicationAsync(publicationDto);

            return RedirectToAction("All");
        }


        [AuthorizeSession(Roles.Lecturer, Roles.HeadOfDepartment)]
        [HttpGet("upload-monograph")]
        public async Task<IActionResult> UploadMonograph()
        {
            var disciplines = (await apiService.GetDisciplinesListAsync());
            var authors = await apiService.GetPeopleAsync();
            var publication = new PostScientificMonographDto();
            publication.PublicationDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            ViewBag.Disciplines = disciplines;
            ViewBag.Authors = authors;

            return View(publication);
        }

        [AuthorizeSession(Roles.Lecturer, Roles.HeadOfDepartment)]
        [HttpPost("upload-monograph")]
        public async Task<IActionResult> UploadMonograph(PostScientificMonographDto publicationDto)
        {
            if (!ModelState.IsValid)
                return View(publicationDto);

            await apiService.PostScientificMonographAsync(publicationDto);

            return RedirectToAction("All");
        }


        [AuthorizeSession(Roles.Lecturer, Roles.HeadOfDepartment)]
        [HttpGet("upload-dissertation")]
        public async Task<IActionResult> UploadDissertation()
        {
            var disciplines = (await apiService.GetDisciplinesListAsync());
            var authors = await apiService.GetPeopleAsync();
            var publication = new PostScientificDissertationDto();
            publication.PublicationDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            ViewBag.Disciplines = disciplines;
            ViewBag.Authors = authors;

            return View(publication);
        }

        [AuthorizeSession(Roles.Lecturer, Roles.HeadOfDepartment)]
        [HttpPost("upload-dissertation")]
        public async Task<IActionResult> UploadDissertation(PostScientificDissertationDto publicationDto)
        {
            if (!ModelState.IsValid)
                return View(publicationDto);

            await apiService.PostScientificDissertationAsync(publicationDto);

            return RedirectToAction("All");
        }


        [AuthorizeSession(Roles.Lecturer, Roles.HeadOfDepartment)]
        [HttpGet("upload-patent")]
        public async Task<IActionResult> UploadPatent()
        {
            var disciplines = (await apiService.GetDisciplinesListAsync());
            var authors = await apiService.GetPeopleAsync();
            var publication = new PostScientificPatentDto();
            publication.PublicationDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            ViewBag.Disciplines = disciplines;
            ViewBag.Authors = authors;

            return View(publication);
        }

        [AuthorizeSession(Roles.Lecturer, Roles.HeadOfDepartment)]
        [HttpPost("upload-patent")]
        public async Task<IActionResult> UploadPatent(PostScientificPatentDto publicationDto)
        {
            if (!ModelState.IsValid)
                return View(publicationDto);

            await apiService.PostScientificPatentAsync(publicationDto);

            return RedirectToAction("All");
        }


        [AuthorizeSession(Roles.Lecturer, Roles.HeadOfDepartment)]
        [HttpGet("upload-conference-theses")]
        public async Task<IActionResult> UploadConferenceTheses()
        {
            var disciplines = (await apiService.GetDisciplinesListAsync());
            var authors = await apiService.GetPeopleAsync();
            var publication = new PostScientificConferenceThesesDto();
            publication.PublicationDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            ViewBag.Disciplines = disciplines;
            ViewBag.Authors = authors;

            return View(publication);
        }

        [AuthorizeSession(Roles.Lecturer, Roles.HeadOfDepartment)]
        [HttpPost("upload-conference-theses")]
        public async Task<IActionResult> UploadConferenceTheses(PostScientificConferenceThesesDto publicationDto)
        {
            if (!ModelState.IsValid)
                return View(publicationDto);

            await apiService.PostScientificConferenceThesesAsync(publicationDto);

            return RedirectToAction("All");
        }

        [AuthorizeSession(Roles.Lecturer, Roles.HeadOfDepartment)]
        [HttpGet("DeletePub")]
        public async Task<IActionResult> DeletePub(int id)
        {
            await apiService.DeletePublication(id);
            return RedirectToAction("Index", "Lecturers", new { id = sessionService.GetUser().user.Person.Id });
        }



    }

}
