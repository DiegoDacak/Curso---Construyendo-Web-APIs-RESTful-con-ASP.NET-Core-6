using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MoviesApi.Common.Messages;
using MoviesApi.Entities.Interfaces;

namespace MoviesApi.Entities
{
    public class Gender : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(40, ErrorMessage = ErrorMessages.StringLenght)]
        public string Name { get; set; }
        public List<MovieGender> MovieGenders { get; set; }
    }
}