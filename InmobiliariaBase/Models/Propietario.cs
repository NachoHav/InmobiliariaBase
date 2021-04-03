using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaBase.Models
{
    public class Propietario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public string Telefono { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Clave { get; set; } 
    }
}
