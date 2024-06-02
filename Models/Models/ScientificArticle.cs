using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;

namespace Models.Models
{
    internal static class Extensions
    {
        internal static string GetDisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var displayAttribute = (DisplayAttribute)Attribute.GetCustomAttribute(field, typeof(DisplayAttribute));

            return displayAttribute?.Name ?? value.ToString();
        }
    }

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
        public override string BibliographicReference =>
            $"{string.Join(", ", Authors.Select(a => a.ShortName))}, {Title} // {JournalDetails}, {PublicationDate.Value.Year} - {JournalType.GetDisplayName()} - {this.Volume} с. {(URL != null ? $"- Режим доступу: {URL}" : "")}";
    }
}
