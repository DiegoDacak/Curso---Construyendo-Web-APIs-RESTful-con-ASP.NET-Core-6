using System;
using System.ComponentModel.DataAnnotations;
using MoviesApi.Common.Messages;

namespace MoviesApi.DTOs.Movie
{
    public class MoviePatchDto
    {
        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(300, ErrorMessage = ErrorMessages.StringLenght)]
        public string Title { get; set; }
        public bool AtCinema { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}