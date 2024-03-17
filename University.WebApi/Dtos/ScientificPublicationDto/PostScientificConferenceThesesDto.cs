using Models.Models;
using System.ComponentModel.DataAnnotations;

namespace University.WebApi.Dtos.ScientificPublicationDto
{
    public class PostScientificConferenceThesesDto
    {
        [Display(Name = "Назва тез")]
        public string Title { get; set; }

        [Display(Name = "Дата проведення конференції")]
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

        [Display(Name = "Місце проведення конференції")]
        public string ConferencePlace { get; set; }

        [Display(Name = "Назва конференції")]
        public string ConferenceName { get; set; }

        [Display(Name = "УДК")]
        public string UDC { get; set; }

        [Display(Name = "DOI")]
        public string? DOI { get; set; }

        [Display(Name = "URL")]
        public string? URL { get; set; }
    }
}
