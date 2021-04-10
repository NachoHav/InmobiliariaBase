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
        public int Id { get; set; }
        public DateTime FechaDesde { get; set; }    
        public DateTime FechaHasta { get; set; }

        public bool Estado { get; set; }

        [Display(Name = "Inmueble")]
        public int InmuebleId { get; set; }


        [Display(Name = "Inquilino")]
        public int InquilinoId { get; set; }

        public Inquilino Inquilino { get; set; }

        public Inmueble Inmueble { get; set; }
    }
}
