using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

namespace WebAPIAutores.DTOs
{
    public class CredencialesUsuario
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}