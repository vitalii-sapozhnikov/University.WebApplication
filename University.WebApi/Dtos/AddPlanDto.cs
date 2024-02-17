using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.WebApi.Dtos
{
    public class AddPlanDto
    {
        public string Title { get; set; }
        public List<AddPublicationToPlanDto> Publications { get; set; }
        public int Year { get; set; }
    }
}
