using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;

namespace Models.Models
{
    public class Person
    {
        public int Id { get; set; }


        [NotMapped]
        public string FullName { get => $"{LastName} {FirstName} {MiddleName}"; }
        [NotMapped]
        public string ShortName { get => $"{LastName} {FirstName.First()}. {MiddleName.First()}."; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }


        // Many to many Publications
        public virtual ICollection<Publication> Publications { get; set; }
    }
}
