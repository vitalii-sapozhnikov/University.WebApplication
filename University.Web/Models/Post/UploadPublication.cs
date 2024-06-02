using Microsoft.AspNetCore.Http;
using Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Web.Models.Post
{
    public class UploadPublication
    {
        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Назва обов'язкова")]
        public string Title { get; set; }

        [Display(Name = "Анотація")]
        [Required(ErrorMessage = "Анотація обов'язкова")]
        public string Abstract { get; set; }

        [Display(Name = "Опис")]
        [Required(ErrorMessage = "Опис обов'язковий")]
        public string Description { get; set; }

        [Display(Name = "Ключові слова")]
        [Required(ErrorMessage = "Ключові слова обов'язкові")]
        public string Keywords { get; set; }

        [Display(Name = "Автор")]
        public string? Authors { get; set; }

        [Display(Name = "Файл")]
        [Required(ErrorMessage = "Прикріпіть файл")]
        public IFormFile File { get; set; }

        [Display(Name = "Дата публікації")]
        public DateTime? PublicationDate { get; set; }


        [Display(Name = "Дисципліни")]
        [Required(ErrorMessage = "Оберіть дисципліни")]
        public int[] DisciplinesIds { get; set; }


        [Display(Name = "Тип")]
        public PublicationType PublicationType { get; set; }

        [Display(Name = "Обсяг, сторінки")]
        public int Volume { get; set; }


        [Display(Name = "Мова")]
        public Language Language { get; set; }
    }
}
