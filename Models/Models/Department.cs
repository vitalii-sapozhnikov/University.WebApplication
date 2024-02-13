using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.WebApi.Models;

namespace Models.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }

        // One to mane Lecturers
        public ICollection<Lecturer> Lecturers { get; set; }

        public int HeadOfDepartmentId { get; set; }
        public HeadOfDepartment HeadOfDepartment { get; set; }

    }
}
