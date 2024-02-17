using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public enum PreparationLevel
    {
        [Display(Name = "Бакалавр")] Bachelor,
        [Display(Name = "Магістр")] Master,
        [Display(Name = "Аспірант")] Postgraduate
    }
    public class WorkPlan
    {
        public int Id { get; set; }

        // Foreign keys
        public int SpecialityId { get; set; }
        public int DepartmentId { get; set; }
        public int EducationYearId { get; set; }


        // Navigation Properties
        public Speciality Speciality { get; set; }
        public Department Department { get; set; }
        public EducationYear EducationYear { get; set; }


        // Collection navigation property for disciplines
        public ICollection<Discipline> Disciplines { get; set; }


        // Other Properties
        public int Course { get; set; }
        public PreparationLevel PreparationLevel { get; set; }

    }
}
