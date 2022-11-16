using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs.Cinema
{
    public class NearbyCinemaFilterDto
    {
        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Range(-180, 180)]
        public double Longitude { get; set; }

        private int _distanceInKm = 10;
        private const int MaxDistanceInKm = 50;

        public int DistanceInKm
        {
            get => _distanceInKm;
            set => _distanceInKm = (value > MaxDistanceInKm) ? MaxDistanceInKm : value;
        }
    }
}