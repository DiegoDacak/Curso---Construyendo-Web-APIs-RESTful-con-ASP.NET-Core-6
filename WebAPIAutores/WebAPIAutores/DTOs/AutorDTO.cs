﻿using System.Collections.Generic;

namespace WebAPIAutores.DTOs
{
    public class AutorDTO: Recurso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}