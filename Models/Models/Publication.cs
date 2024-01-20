using Models.Models;
using System.ComponentModel.DataAnnotations;

namespace University.WebApi.Models
{
    public class Publication
    {
        public int PublicationId { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string CloudStorageGuid { get; set; }
        public string Abstract { get; set; }
        public string Description { get; set; }
        public string[] Keywords { get; set; }

        // Navigation property for many-to-many relationship with authors
        public virtual ICollection<Author> Authors { get; set; }

        // Navigation property for one-to-many relationship with users
        public ApplicationUser User { get; set; }
    }
}