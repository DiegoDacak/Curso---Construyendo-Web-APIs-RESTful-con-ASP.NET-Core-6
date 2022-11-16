using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPIAutores.Validations;

namespace WebAPIAutores.Entidades
{
    public class Autor
    {
        public int Id { get; set; }
        [Required (ErrorMessage = "El {0} es requerido")]
        [StringLength(maximumLength:120, ErrorMessage = "El {0} puede tener un maximo de {1} caracteres")]
        [PrimerLetraMayuscua]
        public string Nombre { get; set; }

        public List<AutorLibro> AutoresLibros { get; set; }
    }
}