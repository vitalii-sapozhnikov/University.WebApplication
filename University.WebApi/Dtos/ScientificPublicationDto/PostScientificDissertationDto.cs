using Models.Models;
using System.ComponentModel.DataAnnotations;

namespace University.WebApi.Dtos.ScientificPublicationDto
{
    public class PostScientificDissertationDto
    {
        [Display(Name = "УДК")]
        public string UDC { get; set; }

        [Display(Name = "DOI")]
        public string DOI { get; set; }

        [Display(Name = "Назва дисертації")]
        public string Title { get; set; }

        [Display(Name = "Дата")]
        public DateTime PublicationDate { get; set; }

        [Display(Name = "Анотація")]
        public string Abstract { get; set; }

        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Display(Name = "Ключові слова")]
        public string Keywords { get; set; }

        [Display(Name = "Обсяг, сторінки")]
        public int Volume { get; set; }

        [Display(Name = "Мова")]
        public Language Language { get; set; }

        [Display(Name = "Автори")]
        public int[] AuthorIds { get; set; }

        [Display(Name = "Дисципліни")]
        public int[] DisciplinesIds { get; set; }

        [Display(Name = "Заклад, у якому захищена дисертація")]
        public string EducationalInstitution { get; set; }

        [Display(Name = "Посилання")]
        public string? URL { get; set; }

        [Display(Name = "Тип дисертації")]
        public DissertationType DissertationType { get; set; }
    }
}
