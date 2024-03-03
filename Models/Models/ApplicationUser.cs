using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;

namespace Models.Models
{
    public class ApplicationUser: Microsoft.AspNetCore.Identity.IdentityUser
    {
        public Person Person { get; set; }
    }
}
