using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.WebApi.Dtos
{
    public class LecturerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public ICollection<Department> Departments{ get; set; }
        public AcademicTitle AcademicTitle { get; set; }
    }
}
