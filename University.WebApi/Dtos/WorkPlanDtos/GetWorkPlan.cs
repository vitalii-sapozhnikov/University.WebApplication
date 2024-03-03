using Models.Models;

namespace University.WebApi.Dtos.WorkPlanDtos
{
    public class GetWorkPlan
    {
        public int Id { get; set; }

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
