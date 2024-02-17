using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Tokens;
using University.Web.Services.Contracts;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace University.Web.Controllers;

[Route("[controller]")]
public class LecturersController : Controller
{
    private readonly IApiService apiService;

    public LecturersController(IApiService apiService)
    {
        this.apiService = apiService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(int id, int? years, DateOnly? beginDate, DateOnly? endDate)
    {
        var result = await apiService.GetLecturerAsync(id);
        if (result.Success)
        {
            var lecturer = result.Data;

            if(years.HasValue)
            {
                lecturer.Publications = lecturer.Publications.Where(p => p.PublicationDate > DateTime.UtcNow.AddYears(-years.Value)).ToList();
            }
            else
            {
                if (beginDate.HasValue)
                    lecturer.Publications = lecturer.Publications.Where(p => DateOnly.FromDateTime(p.PublicationDate.Value) > beginDate.Value).ToList();
                if (endDate.HasValue)
                    lecturer.Publications = lecturer.Publications.Where(p => DateOnly.FromDateTime(p.PublicationDate.Value) < endDate.Value).ToList();
            }

            return View(lecturer);
        }

        return BadRequest("Author not found: " + result.ErrorMessage);
    }

    [HttpGet("GetAuthors")]
    public async Task<IActionResult> GetAuthorsFromDatabase(string term)
    {
        var result = await apiService.GetPeopleAsync();

        if (result == null)
            return BadRequest();

        var authors = result
            .Where(a => a.FirstName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        a.LastName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        a.MiddleName.Contains(term, StringComparison.OrdinalIgnoreCase))
            .Select(a => $"{a.LastName} {a.FirstName} {a.MiddleName}")
            .ToList();

        return Ok(authors);
    }

    
    [HttpGet("Departmental-Lecturers")]
    public async Task<IActionResult> DepartmentalLecturers()
    {
        var lecturersResult = await apiService.GetLecturersAsync();
        var departmetnIdResult = await apiService.GetCurrentDepartmentId();
        var departments = await apiService.GetDepartmentsAsync();

        if(lecturersResult.Success && departmetnIdResult.Success && departments.Success) 
        {
            var lecturers = lecturersResult.Data.Where(l => l.Departments.Any(d => d.DepartmentId == departmetnIdResult.Data)).ToList();
            var departmentName = departments.Data.First(d => d.DepartmentId == departmetnIdResult.Data).Name;
            return View((lecturers, departmentName));
        }

        return BadRequest();
    }

}