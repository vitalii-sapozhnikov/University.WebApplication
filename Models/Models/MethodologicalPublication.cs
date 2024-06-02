using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;

namespace Models.Models
{
    public enum PublicationType
    {
        [Display(Name = "Підручники")] Textbook,
        [Display(Name = "Навчальні посібники")] Tutorial,
        [Display(Name = "Курси лекцій")] LectureCourses,
        [Display(Name = "Навчально-методичні посібники")] EducationalMethodicalManuals,
        [Display(Name = "Практикуми")] Practicums,
        [Display(Name = "Методичні посібники")] MethodicalManuals,
        [Display(Name = "Методичні рекомендації")] MethodicalRecommendations,
        [Display(Name = "Хрестоматії")] Anthologies,
        [Display(Name = "Словники")] Dictionaries,
        [Display(Name = "Довідники")] Handbooks,
        [Display(Name = "Робоча програма")] WorkProgramme
    }
    public class MethodologicalPublication: Publication
    {
        public PublicationType? Type { get; set; }
        public string? CloudStorageGuid { get; set; }

        public int? PlanId { get; set; }
        public Plan Plan { get; set; }

        public override string BibliographicReference =>
            $"{string.Join(", ", Authors.Select(a => a.ShortName))}, {Title}: {Type?.GetDisplayName()}, {(PublicationDate.HasValue ? PublicationDate.Value.Year.ToString() : "")}. - {Volume} с.";
    }
}
