using Microsoft.AspNetCore.Identity;
using WebAPIAutores.Migrations;

namespace WebAPIAutores.Entidades
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Contenido { get; set; }
        public int LibroId { get; set; }
        public Libro Libro { get; set; }
        public int UsuarioId { get; set; }
        public IdentityUser Usuario { get; set; }
    }
}