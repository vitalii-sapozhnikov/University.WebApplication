using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.Roles;
using System.Net.Mime;
using University.Web.Services;
using University.Web.Services.Contracts;
using University.WebApi.Dtos;
using University.WebApi.Dtos.MethodologicalPublicationDto;

namespace University.Web.Controllers
{
    [Route("[controller]")]
    public class PlansController : Controller
    {
        private readonly IApiService apiService;

        public PlansController(IApiService apiService)
        {
            this.apiService = apiService;
        }

        [AuthorizeSession(Roles.HeadOfDepartment, Roles.Lecturer, Roles.EducationDepartment)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await apiService.GetPlansAsync();
            if(result.Success)
                return View(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [AuthorizeSession(Roles.HeadOfDepartment, Roles.EducationDepartment)]
        [HttpGet("download")]
        public async Task<IActionResult> Download(int id)
        {
            var result = await apiService.DownloadPlanAsync(id);
            if (result.Success)
            {
                string contentType = "application/pdf";

                var contentDisposition = new ContentDisposition
                {
                    Inline = true
                };

                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                return File(result.Data, contentType);
            }
            return BadRequest(result.ErrorMessage);
        }

        [AuthorizeSession(Roles.HeadOfDepartment)]
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [AuthorizeSession(Roles.HeadOfDepartment)]
        [HttpPost("create")]
        public async Task<IActionResult> Create(AddPlanDto model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await apiService.AddPlanAsync(model);
            if(!result.Success)
                return BadRequest(result.ErrorMessage);           


            return RedirectToAction("Index");
        }

        [AuthorizeSession(Roles.HeadOfDepartment, Roles.Lecturer, Roles.EducationDepartment)]
        [HttpGet("info/{id}")]
        public async Task<IActionResult> Info(int id)
        {            
            var result = await apiService.GetPlanInfoAsync(id);
            if(!result.Success) { return RedirectToAction("Index"); }

            return View(result.Data);
        }

        [AuthorizeSession(Roles.HeadOfDepartment, Roles.Lecturer, Roles.EducationDepartment)]
        [HttpPost("edit/{publicationId}&{planId}")]
        public async Task<IActionResult> Edit(AddPublicationToPlanDto publication, int publicationId, int planId)
        {
            var result = await apiService.EditPublication(new MethodologicalPublicationDto
            {
                Title = publication.Title,  
                Volume = publication.Volume,
                Language = publication.Language,
                Type = publication.Type,
                LecturerIds = publication.LecturerIds,
                isPublished = false
            }, publicationId);

            return RedirectToAction("Info", new { id = planId });
        }
        
    }
}
