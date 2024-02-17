using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class HeadOfDepartment: Person
    {
        public ApplicationUser ApplicationUser { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
