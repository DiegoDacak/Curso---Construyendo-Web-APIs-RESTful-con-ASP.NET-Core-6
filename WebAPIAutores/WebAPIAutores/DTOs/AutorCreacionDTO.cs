using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validations;

namespace WebAPIAutores.DTOs
{
    public class AutorCreacionDTO
    {
        [Required (ErrorMessage = "El {0} es requerido")]
        [StringLength(maximumLength:120, ErrorMessage = "El {0} puede tener un maximo de {1} caracteres")]
        [PrimerLetraMayuscua]
        public string Nombre  { get; set; }
    }
}