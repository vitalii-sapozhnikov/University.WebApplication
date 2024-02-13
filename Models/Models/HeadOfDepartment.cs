using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class HeadOfDepartment
    {
        public int HeadOfDepartmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
