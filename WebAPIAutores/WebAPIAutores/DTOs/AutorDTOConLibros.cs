﻿using System.Collections.Generic;

namespace WebAPIAutores.DTOs
{
    public class AutorDTOConLibros : AutorDTO
    {
        public List<LibroDTO> Libros { get; set; }
    }
}