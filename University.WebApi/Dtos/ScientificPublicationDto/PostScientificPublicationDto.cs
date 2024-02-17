using Models.Models;
using System.ComponentModel.DataAnnotations;

namespace University.WebApi.Dtos.ScientificPublicationDto
{
    public class PostScientificPublicationDto
    {
        [Display(Name = "Тип журналу")]
        public JournalType JournalType { get; set; }

        [Display(Name = "Деталі журналу (назва, номер, серія, випуск)")]
        public string JournalDetails { get; set; }

        [Display(Name = "Посилання DOI")]
        public string DOI { get; set; }

        [Display(Name = "Назва статті")]
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
    }
}
