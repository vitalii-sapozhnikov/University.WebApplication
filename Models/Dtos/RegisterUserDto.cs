using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class RegisterUserDto
    {        
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name ="Електронна адреса")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name ="Прізвище")]
        public string LastName { get; set; }

        [Required]
        [Display(Name ="По-батькові")]
        public string MiddleName { get; set; }

        public string Role { get; set; } = Roles.Roles.User;
    }
}
