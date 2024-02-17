using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;

namespace Models.Models
{
    public class Discipline
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Publication> Publications { get; set; }
        public virtual ICollection<LecturerDiscipline> LecturerDisciplines { get; set; }
    }
}
