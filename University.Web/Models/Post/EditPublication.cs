using Models.Models;
using System.ComponentModel.DataAnnotations;

namespace University.Web.Models.Post
{
    public class EditPublication
    {
        public int Id { get; set; }

        [Display(Name ="Назва")]
        [Required]
        public string? Title { get; set; }
        
        [Display(Name ="Анотація")]
        [Required]
        public string? Abstract { get; set; }
        
        [Display(Name ="Опис")]
        [Required]
        public string? Description { get; set; }


        [Display(Name = "Файл")]
        public IFormFile File { get; set; }

        [Display(Name ="Ключові слова")]
        [Required]
        public string Keywords { get; set; }

        [Display(Name ="Об'єм")]
        [Required]
        public int? Volume { get; set; }

        [Display(Name ="Мова")]
        [Required]
        public Language? Language { get; set; }

        [Display(Name ="Тип публікації")]
        [Required]
        public PublicationType? Type { get; set; }
    }
}
