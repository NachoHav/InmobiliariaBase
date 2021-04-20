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

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Required]
        public DateTime FechaDesde { get; set; }
        [Required]

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]

        public DateTime FechaHasta { get; set; }
        [Required]

        public int Estado { get; set; }
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
    }
}
