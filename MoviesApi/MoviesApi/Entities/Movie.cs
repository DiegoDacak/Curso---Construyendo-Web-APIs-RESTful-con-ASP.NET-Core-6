using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MoviesApi.Common.Messages;
using MoviesApi.Entities.Interfaces;

namespace MoviesApi.Entities
{
    public class Movie : IId
    {
        public int Id { get; set; }
        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(300, ErrorMessage = ErrorMessages.StringLenght)]
        public string Title { get; set; }
        public bool AtCinema { get; set; }
        public DateTime ReleaseDat { get; set; }
        public string Poster { get; set; }
        public List<MovieActor> MovieActors { get; set; }
        public List<MovieGender> MovieGenders { get; set; }
        public List<MoviesCinema> MoviesCinemas { get; set; }
    }
}