using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.AdditionalTypes
{
    public class LecturerLicense
    {
        public Lecturer Lecturer { get; set; }
        public Dictionary<string, string[]> ConditionPublications { get; set; }

    }
}
