using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs.Cinema
{
    public class CinemaDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Range(-180, 180)]
        public double Longitude { get; set; }
    }
}