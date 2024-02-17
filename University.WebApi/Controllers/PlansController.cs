using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.Roles;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using University.WebApi.Contexts;
using University.WebApi.Dtos;
using Newtonsoft.Json;
using System.Text;
using System.Numerics;
using University.WebApi.Mapping;
using Npgsql.Internal;
using iText.Kernel.Pdf;
using iText.Layout.Font;
using iText.Kernel.Font;
using iText.Html2pdf;

namespace University.WebApi.Controllers
{
    [Authorize(Roles = "HeadOfDepartment, Lecturer, EducationDepartment")]
    [Route("api/[controller]")]
    [ApiController]
    public class PlansController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public PlansController(AppDbContext appDbContext, UserManager<ApplicationUser> userManager)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
        }

        [Authorize(Roles = "HeadOfDepartment")]
        [HttpPost("add")]
        public async Task<IActionResult> AddPlan(AddPlanDto model)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null) { return NotFound("User not found!"); }
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null) { return NotFound("User not found!"); }

            int departmentId = appDbContext.HeadsOfDepartments.First(h => h.ApplicationUser.Id == user.Id).DepartmentId;

            Plan plan = new Plan
            {
                Title = model.Title,
                Year = model.Year,
                DepartmentId = departmentId,
                MethodologicalPublications = new List<MethodologicalPublication>()
            };
            for (int i = 0; i < model.Publications.Count; i++)
            {
                var publicationDto = model.Publications[i];
                plan.MethodologicalPublications.Add(new MethodologicalPublication
                {
                    Title = publicationDto.Title,
                    Volume = publicationDto.Volume,
                    Language = publicationDto.Language,
                    Type = publicationDto.Type,
                    Authors = appDbContext.Lecturers
                        .Where(l => publicationDto.LecturerIds.Contains(l.Id))
                        .Select(l => l as Person)
                        .ToList(),
                    isPublished = false
                });
            }

            await appDbContext.Plans.AddAsync(plan);

            await appDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetPlans()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null) { return NotFound("User not found!"); }
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null) { return NotFound("User not found!"); }

            var userRoles = await userManager.GetRolesAsync(user);
            int departmentId = -1;
            int userId = -1;

            switch (userRoles.First())
            {
                case Roles.HeadOfDepartment:
                    departmentId = appDbContext.HeadsOfDepartments
                        .First(h => h.ApplicationUser == user).DepartmentId;
                    userId = appDbContext.HeadsOfDepartments
                        .First(h => h.ApplicationUser == user).Id;
                    break;
                case Roles.Lecturer:
                    departmentId = appDbContext.Lecturers
                        .First(h => h.ApplicationUser == user).Departments.First().DepartmentId;
                    userId = appDbContext.Lecturers
                        .First(h => h.ApplicationUser == user).Id;
                    break;
            }

            IQueryable<Plan> plans = appDbContext.Plans;

            if (departmentId != -1)
            {
                plans = plans.Where(p => p.DepartmentId == departmentId);
            }

            plans = plans
                .Include(p => p.MethodologicalPublications)
                    .ThenInclude(pub => pub.Authors)
                .Include(p => p.Department)
                    .ThenInclude(d => d.HeadOfDepartment);

            if (userRoles.First() == Roles.Lecturer)
            {
                foreach (var plan in plans)
                {
                    plan.MethodologicalPublications = plan.MethodologicalPublications.Where(p => p.Authors.Any(l => l.Id == userId)).ToList();
                }
            }


            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(plans, Formatting.Indented,  settings);

            return Ok(json);
        }

        [HttpGet("get-pdf/{id}")]
        public async Task<IActionResult> GetPlans(int id)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null) { return NotFound("User not found!"); }
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null) { return NotFound("User not found!"); }

            var userRoles = await userManager.GetRolesAsync(user);
            int departmentId = -1;

            switch (userRoles.First())
            {
                case Roles.HeadOfDepartment:
                    departmentId = appDbContext.HeadsOfDepartments
                        .First(h => h.ApplicationUser == user).DepartmentId;
                    break;
                case Roles.Lecturer:
                    departmentId = appDbContext.Lecturers
                        .First(h => h.ApplicationUser == user).Departments.First().DepartmentId;
                    break;
            }

            var plan = appDbContext.Plans
                .Include(p => p.MethodologicalPublications)
                    .ThenInclude(pub => pub.Authors)
                .Include(p => p.Department)
                    .ThenInclude(d => d.HeadOfDepartment)
                .FirstOrDefault(p => p.DepartmentId == departmentId && p.PlanId == id);

            if (plan == null)
                return NotFound();

            var htmlContent = GetHtmlPlanDoc(plan);
            byte[] pdfFile;
            try
            {
                pdfFile = ConvertHtmlStringToPdf(htmlContent.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(pdfFile);
        }

        private StringBuilder GetHtmlPlanDoc(Plan plan)
        {
            var htmlContent = new StringBuilder();
            htmlContent.Append($@"<!DOCTYPE html><html lang=""en""><head> <meta charset=""UTF-8""> <meta name=""viewport"" content=""width=device-width, initial-scale=1.0""> <title>Document</title> <style> table {{ border: 1px solid black; border-collapse: collapse; font-family: 'Times New Roman', Times, serif; /* ensures borders don't overlap between cells */ }} th, td {{ border: 1px solid black; padding: 5px; /* adjust for desired padding within cells */ }} </style></head><body> <h3 style=""text-align: center;"">{plan.Department.Name}</h3><br><table> <tbody> <tr> <th colspan=""4"">Автор, назва видання, мова видання, обсяг (в авт. арк.)</th> </tr>");

            const int cols = 4;
            var publicationTypes = Enum.GetValues<PublicationType>();
            int rowsCount = (int)Math.Ceiling((double)publicationTypes.Length / cols);

            for (int i = 0; i < rowsCount; i++)
            {
                htmlContent.Append(@"<tr><td colspan=""4""></td></tr>");

                htmlContent.Append(@"<tr>");
                for (int j = i * cols; j < i * cols + cols; j++)
                {
                    if (j < publicationTypes.Length)
                    {
                        string category = publicationTypes[j].GetDisplayName();
                        htmlContent.Append(@$"<th>{category}</th>");
                    }
                    else
                    {
                        htmlContent.Append(@$"<th></th>");
                    }
                }
                htmlContent.Append(@"</tr>");

                htmlContent.Append(@"<tr>");
                for (int j = i * cols; j < i * cols + cols; j++)
                {
                    if (j < publicationTypes.Length)
                    {
                        htmlContent.Append(@"<td>");
                        int c = 0;
                        foreach (var pub in plan.MethodologicalPublications.Where(p => p.Type == publicationTypes[j]))
                        {
                            var temp = pub.Authors.Count() > 1 ? "Укладачі" : "Укладач";
                            htmlContent.Append(@$"{++c}. {pub.Title} / {temp} {String.Join(", ", pub.Authors.Select(l => l.ShortName))} - {pub?.Language?.GetDisplayName()} ({pub?.Volume} др. арк.)");
                            if (pub != plan.MethodologicalPublications.Where(p => p.Type == publicationTypes[j]).Last())
                            {
                                htmlContent.Append("<br><br>");
                            }
                        }
                        htmlContent.Append(@"</td>");
                    }
                    else
                    {
                        htmlContent.Append(@$"<td></td>");
                    }
                }
                htmlContent.Append(@"</tr>");
            }

            htmlContent.Append(@$"</tbody> </table><br><div style=""display: flex; align-items: flex-end; justify-content: space-between;""> <p>Затверджено на засіданні кафедри<br>Завкафедрою</p> <p>{plan.Department.HeadOfDepartment.ShortName}</p> </div></body></html>");

            return htmlContent;
        }

        static byte[] ConvertHtmlStringToPdf(string htmlContent)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create PdfWriter and PdfDocument
                    PdfWriter pdfWriter = new PdfWriter(ms);
                    PdfDocument pdfDocument = new PdfDocument(pdfWriter);

                    pdfDocument.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4.Rotate());

                    FontProvider fontProvider = new FontProvider();
                    PdfFont font = PdfFontFactory.CreateFont("C:\\Users\\vital\\Documents\\Универ\\4 курс\\Диплом\\University.WebApi\\Models\\Fonts\\TIMES.TTF");
                    PdfFont font2 = PdfFontFactory.CreateFont("C:\\Users\\vital\\Documents\\Универ\\4 курс\\Диплом\\University.WebApi\\Models\\Fonts\\TIMESBD.TTF");
                    fontProvider.AddFont(font.GetFontProgram());
                    fontProvider.AddFont(font2.GetFontProgram());

                    // Convert HTML string to PDF
                    ConverterProperties converterProperties = new ConverterProperties();
                    converterProperties.SetFontProvider(fontProvider);
                    HtmlConverter.ConvertToPdf(htmlContent, pdfDocument, converterProperties);

                    Console.WriteLine("Conversion successful!");

                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}
