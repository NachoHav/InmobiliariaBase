using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaBase.Models
{

    public enum Roles
    {
        SuperAdmin = 1,
        Admin = 2,
        Employee = 3,
    } 
    public class Usuario
    {
        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Rol { get; set; }
        public string Clave { get; set; }
        public string Avatar { get; set; }

    }
}
