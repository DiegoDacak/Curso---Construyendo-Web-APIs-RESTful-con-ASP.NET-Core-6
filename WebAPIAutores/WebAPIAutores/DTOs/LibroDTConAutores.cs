using System.Collections.Generic;

namespace WebAPIAutores.DTOs
{
    public class LibroDTConAutores : LibroDTO
    {
        public List<AutorDTO> Autores { get; set; }   
    }
}