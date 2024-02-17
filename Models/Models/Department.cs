using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;

namespace Models.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }

        // Navigation properties
        public HeadOfDepartment HeadOfDepartment { get; set; }


        // Collection navigation properties
        public ICollection<Lecturer> Lecturers { get; set; }
        public ICollection<Plan> Plans { get; set; }
    }
}
