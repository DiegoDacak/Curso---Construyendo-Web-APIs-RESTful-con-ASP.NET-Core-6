using System.Collections.Generic;

namespace MoviesApi.DTOs.Movie
{
    public class MoviesIndexDto
    {
        public List<MovieDto> FutureRelease { get; set; }
        public List<MovieDto> AtCinema { get; set; }
    }
}