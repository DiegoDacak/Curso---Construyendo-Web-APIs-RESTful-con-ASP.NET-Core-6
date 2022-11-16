using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validations;

namespace WebAPIAutores.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 250)]
        [PrimerLetraMayuscua]
        public string Title { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public List<Comentario> Comentarios { get; set; }
        public List<AutorLibro> AutoresLibros { get; set; }
    }
}