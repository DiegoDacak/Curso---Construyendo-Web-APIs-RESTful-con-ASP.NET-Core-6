using System.Collections.Generic;
using MoviesApi.DTOs.Gender;

namespace MoviesApi.DTOs.Movie
{
    public class MovieDetailDto : MovieDto
    {
        public List<GenderDto> Gender { get; set; }
        public List<MovieActorDetailDto> Actor { get; set; }
    }
}