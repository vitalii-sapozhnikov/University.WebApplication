using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;

namespace Models.Models
{
    public enum JournalType
    {
        [Display(Name = "Scopus")]
        Scopus,
        [Display(Name = "Web of Science")]
        WebOfScience,
        [Display(Name = "Категорія Б")]
        Category2,
        [Display(Name = "Категорія В")]
        Category3
    }
    public class ScientificArticle: ScientificPublication
    {
        public JournalType JournalType { get; set; }
        public string JournalDetails { get; set; }
    }
}
