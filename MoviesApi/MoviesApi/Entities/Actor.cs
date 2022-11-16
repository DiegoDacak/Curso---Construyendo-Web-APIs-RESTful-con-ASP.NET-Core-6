using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MoviesApi.Common.Messages;
using MoviesApi.Entities.Interfaces;

namespace MoviesApi.Entities
{
    public class Actor : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120, ErrorMessage = ErrorMessages.StringLenght)]
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Photo { get; set; }
        public List<MovieActor> MovieActors { get; set; }
        
    }
}