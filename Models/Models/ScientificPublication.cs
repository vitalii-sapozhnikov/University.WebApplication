using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class ScientificPublication: Publication
    {
        public string UDC { get; set; }
        public string? DOI { get; set; }
        public string? URL { get; set; }
    }
}
