using System;

namespace MoviesApi.DTOs.Movie
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool AtCinema { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
    }
}