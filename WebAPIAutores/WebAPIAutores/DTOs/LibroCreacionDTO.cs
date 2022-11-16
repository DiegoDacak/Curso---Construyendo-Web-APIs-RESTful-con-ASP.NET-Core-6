using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validations;

namespace WebAPIAutores.DTOs
{
    public class LibroCreacionDTO
    {
        [StringLength(maximumLength: 250)]
        [PrimerLetraMayuscua]
        public string Titulo { get; set; }

        public DateTime FechaPubllicacion { get; set; }
        public List<int> AutoresIds { get; set; }
    }
}