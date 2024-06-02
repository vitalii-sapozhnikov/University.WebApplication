using Models.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Models
{
    public enum Language { [Display(Name = "укр. мова")]Ukrainian, [Display(Name ="eng.")]English}
    
    public class Publication
    {
        public int PublicationId { get; set; }
        public string Title { get; set; }
        public DateTime? PublicationDate { get; set; }        
        public string? Abstract { get; set; }
        public string? Description { get; set; }        
        public string[]? Keywords { get; set; }
        public bool isPublished { get; set; } = true;
        public int? Volume { get; set; }
        public Language? Language { get; set; }
        

        // Navigation property for many-to-many relationship with authors
        public virtual ICollection<Person> Authors { get; set; }

        [NotMapped]
        public int[] LecturerIds { get; set; }       

        // Many to many Disciplines
        public virtual ICollection<Discipline> Disciplines { get; set; }

        [NotMapped]
        public virtual string BibliographicReference { get; }
    }
}