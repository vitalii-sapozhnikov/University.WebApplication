using Models.Models;

namespace University.WebApi.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }  
        public virtual ICollection<Publication> Publications { get; set; }
    }
}
