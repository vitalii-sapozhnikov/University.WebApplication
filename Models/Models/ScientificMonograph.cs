using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class ScientificMonograph: ScientificPublication
    {
        public string Publisher { get; set; }
        public string ISBN { get; set; }
    }
}
