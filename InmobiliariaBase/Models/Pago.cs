using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaBase.Models
{
    public class Pago
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "Código Contrato")]
        public int IdContrato { get; set; }
        public Contrato Contrato { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Pago")]
        public DateTime FechaPago { get; set; }

        public bool Estado { get; set; }

        public int Importe { get; set; }

    }
}
