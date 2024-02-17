using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.WebApi.Dtos
{
    public class PublicationShortDto
    {
        public int PublicationId { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Description { get; set; }
        public string[] Keywords { get; set; }
        public virtual ICollection<Person> Lecturers { get; set; }
    }
}
