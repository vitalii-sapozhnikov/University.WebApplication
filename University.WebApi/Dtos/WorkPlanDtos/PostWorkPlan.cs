using Models.Models;

namespace University.WebApi.Dtos.WorkPlanDtos
{
    public class PostWorkPlan
    {
        public int SpecialityId { get; set; }
        public int DepartmentId { get; set; }
        public int EducationYearId { get; set; }
        public string GuarantorId { get; set; }
        public int[] DisciplinesIds { get; set; }
        public int Course { get; set; }
        public PreparationLevel PreparationLevel { get; set; }

    }
}
