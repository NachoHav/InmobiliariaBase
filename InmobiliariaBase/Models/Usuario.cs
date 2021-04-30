using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaBase.Models
{

    public enum Roles
    {        
        SuperAdmin =1,
        Admin = 2,
        Employee = 3,
    } 
    public class Usuario
    {
        [Key]
        [Display(Name = "Código")]
        [Required]
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public int Rol { get; set; }


        [Required, DataType(DataType.Password), StringLength(16, MinimumLength = 8, ErrorMessage = "La clave debe ser de 8 a 16 caracteres.")]
        public string Clave { get; set; }

        public string Avatar { get; set; }

        [Display(Name ="Avatar")]
        public IFormFile AvatarFile { get; set; }
        public bool Estado { get; set; }

        public string RolNombre => Rol > 0 ? ((Roles)Rol).ToString() : "";

        public static IDictionary<int, string> ObtenerRoles()
        {
            SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
            Type tipoEnumRol = typeof(Roles);
            foreach (var valor in Enum.GetValues(tipoEnumRol))
            {
                roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
            }
            return roles;
        }

    }
}
