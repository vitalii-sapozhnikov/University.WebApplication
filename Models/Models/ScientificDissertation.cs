using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public enum DissertationType
    {
        [Display(Name = "Магістерська")]
        Masters,
        [Display(Name = "Кандидатська")]
        Candidates,
        [Display(Name = "Докторська")]
        Doctoral
    }
    public class ScientificDissertation: ScientificPublication
    {
        public string EducationalInstitution { get; set; }
        public DissertationType DissertationType { get; set; }
    }
}
