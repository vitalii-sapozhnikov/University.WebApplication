using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using University.WebApi.Contexts;
using University.WebApi.Mapping;
using Models.Models;
using University.WebApi.Dtos.PersonDtos;
using Models.Models.AdditionalTypes;

namespace University.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public LecturersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpPost("add-multiple")]
        public async Task<IActionResult> AddMultipleLecturers(List<PostPersonDto> lecturerDtos)
        {
            try
            {
                if (lecturerDtos == null || !lecturerDtos.Any())
                {
                    return BadRequest("No lecturers provided.");
                }

                List<Lecturer> lecturers = lecturerDtos.Select(a =>
                    {
                        var lecturer = a.ToLecturerEntity();
                        //TODO: Lecturer Department when adding lecturer
                        //lecturer.DepartmentId = 0;
                        return lecturer;
                    }).ToList();

                // Add the authors to the database
                _dbContext.Lecturers.AddRange(lecturers);
                await _dbContext.SaveChangesAsync();

                return Ok("Lecturers added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddLecturer(PostPersonDto lecturerDto)
        {
            try
            {
                if (lecturerDto == null)
                {
                    return BadRequest("No lecturer provided.");
                }
                Lecturer lecturer = lecturerDto.ToLecturerEntity();
                
                //TODO: Lecturer department when added
                //lecturer.DepartmentId = 0;

                _dbContext.Lecturers.Add(lecturer);
                await _dbContext.SaveChangesAsync();

                return Ok("Lecturer added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllLecturers()
        {
            try
            {
                var lecturers = await _dbContext.Lecturers.Include(l => l.Departments).ToListAsync();
                var lecturerDtos = lecturers.Select(a => a.ToLecturerDto());

                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                var json = JsonConvert.SerializeObject(lecturerDtos, Formatting.Indented, settings);
                return Ok(json);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetLecturer(int id)
        {
            var author = await _dbContext.Lecturers
                .Include(a => a.Publications)
                .FirstOrDefaultAsync(a => a.Id == id);

            author.Publications = author.Publications.Where(p => p.isPublished).ToList();
            
            if (author is null)
                return NotFound();

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            return Ok(JsonConvert.SerializeObject(author, jsonSettings));
        }

        [HttpGet("correlation")]
        public async Task<IActionResult> GetLecturerCorrelation(int lectId, int discId)
        {
            var lecturer = await _dbContext.Lecturers
                .Include(l => l.Publications) // Include the publications
                    .ThenInclude(p => p.Disciplines) // Include the disciplines for each publication
                .FirstOrDefaultAsync(l => l.Id == lectId);

            if (lecturer == null) return NotFound();

            var publications = lecturer.Publications
                .Where(p => p.Disciplines.Any(d => d.Id == discId))
                .ToList();

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto
            };

            string json = JsonConvert.SerializeObject(publications, Formatting.Indented, settings);

            return Ok(json);
        }

        [HttpGet("license")]
        public async Task<IActionResult> GetLecturerLicense(int lectId)
        {
            var lecturer = await _dbContext.Lecturers
                .Include(l => l.Publications)
                    .ThenInclude(p => p.Disciplines)
                .FirstOrDefaultAsync(l => l.Id == lectId);

            if (lecturer == null) return NotFound();

            lecturer.Publications = lecturer.Publications
                .Where(p => p.PublicationDate > DateTime.SpecifyKind(DateTime.Now.AddYears(-5), DateTimeKind.Utc) && p.isPublished)
                .ToList();


            string[] conditions = new string[]
            {
                "1) наявність не менше п’яти публікацій у періодичних наукових виданнях, що включені до переліку фахових видань України, до наукометричних баз, зокрема Scopus, Web of Science Core Collection;",
                "2) наявність одного патенту на винахід або п’яти деклараційних патентів на винахід чи корисну модель, включаючи секретні, або наявність не менше п’яти свідоцтв про реєстрацію авторського права на твір;",
                "3) наявність виданого підручника чи навчального посібника (включаючи електронні) або монографії (загальним обсягом не менше 5 авторських аркушів), в тому числі видані у співавторстві (обсягом не менше 1,5 авторського аркуша на кожного співавтора);",
                "4) наявність виданих навчально-методичних посібників/посібників для самостійної роботи здобувачів вищої освіти та дистанційного навчання, електронних курсів на освітніх платформах ліцензіатів, конспектів лекцій/практикумів/методичних вказівок/рекомендацій/ робочих програм, інших друкованих навчально-методичних праць загальною кількістю три найменування;",
                "12) наявність апробаційних та/або науково-популярних, та/або консультаційних (дорадчих), та/або науково-експертних публікацій з наукової або професійної тематики загальною кількістю не менше п’яти публікацій;"
            };

            LecturerLicense result = new LecturerLicense();
            result.Lecturer = lecturer;

            result.ConditionPublications = new Dictionary<string, string[]>();

            result.ConditionPublications.Add(conditions[0], lecturer.Publications
                .Where(p => p is ScientificArticle)
                .Select(a => a.BibliographicReference)
                .ToArray());

            result.ConditionPublications.Add(conditions[1], lecturer.Publications
                .Where(p => p is ScientificPatent)
                .Select(a => a.BibliographicReference)
                .ToArray());

            result.ConditionPublications.Add(conditions[2], lecturer.Publications
                .Where(p => (p is MethodologicalPublication meth && (meth.Type == PublicationType.Textbook || meth.Type == PublicationType.Tutorial)) || p is ScientificMonograph)
                .Select(a => a.BibliographicReference)
                .ToArray());

            result.ConditionPublications.Add(conditions[3], lecturer.Publications
                .Where(p => (p is MethodologicalPublication meth && (meth.Type != PublicationType.Textbook && meth.Type != PublicationType.Tutorial)))
                .Select(a => a.BibliographicReference)
                .ToArray());

            result.ConditionPublications.Add(conditions[4], lecturer.Publications
                .Where(p => p is ScientificConferenceTheses)
                .Select(a => a.BibliographicReference)
                .ToArray());


            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto
            };

            string json = JsonConvert.SerializeObject(result, Formatting.Indented, settings);

            return Ok(json);
        }
    }
}
