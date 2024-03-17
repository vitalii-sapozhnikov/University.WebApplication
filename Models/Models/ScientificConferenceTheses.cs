using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class ScientificConferenceTheses: ScientificPublication
    {
        public string ConferenceName { get; set; }
        public string ConferencePlace { get; set; }
    }
}
