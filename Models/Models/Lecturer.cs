using Models.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.Models
{
    public enum AcademicTitle
    {
        [Display(Name = "Старший викладач")] SeniorLecturer, 
        [Display(Name = "Доцент")] AssistantProfessor,
        [Display(Name = "Професор")] Professor
    }
    public class Lecturer: Person
    {        
        public AcademicTitle AcademicTitle { get; set; }        

        // One to one ApplicationUser
        public ApplicationUser ApplicationUser { get; set; }

        // Many to many Department
        public ICollection<Department> Departments { get; set; }

        public virtual ICollection<LecturerDiscipline> LecturerDisciplines { get; set; }
    }
}
