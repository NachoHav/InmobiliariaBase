    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaBase.Models
{

    public enum Tipos
    {
        Casa = 1,
        Departamento = 2,
        Local = 3,
        Cochera = 4,
        Galpon = 5,
    }

    public class Inmueble
    {
        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public int Tipo { get; set; }
        [Required]
        public int Ambientes { get; set; }
        [Required]
        public int Superficie { get; set; }

        public float Importe { get; set; }

        [Display(Name = "Dueño")]
        public int PropietarioId { get; set; }
        [ForeignKey("PropietarioId")]
        public Propietario Duenio { get; set; }



        public string TipoNombre => Tipo > 0 ? ((Tipos)Tipo).ToString() : "";

        public static IDictionary<int, string> ObtenerTipos()
        {
            SortedDictionary<int, string> tipos = new SortedDictionary<int, string>();
            Type tipoEnumTipo = typeof(Tipos);
            foreach (var valor in Enum.GetValues(tipoEnumTipo))
            {
                tipos.Add((int)valor, Enum.GetName(tipoEnumTipo, valor));
            }
            return tipos;
        }

    }
}
