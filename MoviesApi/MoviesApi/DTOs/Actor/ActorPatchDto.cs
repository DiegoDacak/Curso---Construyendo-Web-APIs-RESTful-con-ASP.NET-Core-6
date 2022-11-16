using System;
using System.ComponentModel.DataAnnotations;
using MoviesApi.Common.Messages;

namespace MoviesApi.DTOs.Actor
{
    public class ActorPatchDto
    {
        [Required]
        [StringLength(120, ErrorMessage = ErrorMessages.StringLenght)]
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}