﻿    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaBase.Models
{
    public class Inmueble
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        public int Ambientes { get; set; }
        [Required]
        public int Superficie { get; set; }

        [Display(Name = "Dueño")]
        public int PropietarioId { get; set; }
        [ForeignKey("PropietarioId")]
        public Propietario Duenio { get; set; }
    }
}
