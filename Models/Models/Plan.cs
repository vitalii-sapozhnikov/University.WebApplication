using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;

namespace Models.Models
{
    public class Plan
    {
        public int PlanId { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public virtual ICollection<MethodologicalPublication> MethodologicalPublications { get; set; }
    }
}
