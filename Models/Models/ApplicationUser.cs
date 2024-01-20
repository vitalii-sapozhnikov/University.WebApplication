using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.WebApi.Models;

namespace Models.Models
{
    public class ApplicationUser: Microsoft.AspNetCore.Identity.IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Publication> Publications { get; set; }
    }
}
