using System;
using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validations;

namespace WebAPIAutores.DTOs
{
    public class LibroPatchDTO
    {
        [StringLength(maximumLength: 250)]
        [PrimerLetraMayuscua]
        public string Titulo { get; set; }

        public DateTime FechaPubllicacion { get; set; }
    }
}