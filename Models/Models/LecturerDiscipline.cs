using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;

namespace Models.Models
{
    public class LecturerDiscipline
    {
        public int LecturerId { get; set; }
        public Lecturer Lecturer { get; set; }

        public int DisciplineId { get; set; }
        public Discipline Discipline { get; set; }

        public DateOnly BeginDate { get; set; } // Start date of lecturer's involvement
        public DateOnly? EndDate { get; set; }   // End date of lecturer's involvement
    }
}
