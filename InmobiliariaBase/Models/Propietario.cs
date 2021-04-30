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

        [Required, DataType(DataType.Password), StringLength(16, MinimumLength = 8, ErrorMessage = "La clave debe ser de 8 a 16 caracteres.")]
        public string Clave { get; set; } 
    }
}
