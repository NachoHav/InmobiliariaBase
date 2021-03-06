using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaBase.Models
{
    public class Contrato
    {

        [Display(Name = "Código")]
        [Required]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Required]

        [Display(Name = "Fecha Inicio")]
        public DateTime FechaDesde { get; set; }
        [Required]

        [DataType(DataType.Date)]
        [Display(Name = "Fecha Fin")]
        public DateTime FechaHasta { get; set; }
        [Required]

        public bool Estado { get; set; }
        [Required]

        [Display(Name = "Inmueble")]
        public int InmuebleId { get; set; }
        [Required]


        [Display(Name = "Inquilino")]
        public int InquilinoId { get; set; }
        [Required]

        public Inquilino Inquilino { get; set; }
        [Required]

        public Inmueble Inmueble { get; set; }

        public int Importe { get; set; }
    }
}
