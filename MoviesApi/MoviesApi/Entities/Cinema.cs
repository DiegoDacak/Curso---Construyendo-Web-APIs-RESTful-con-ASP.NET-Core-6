using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MoviesApi.Entities.Interfaces;
using NetTopologySuite.Geometries;

namespace MoviesApi.Entities
{
    public class Cinema: IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public Point Location { get; set; }
        public List<Cinema> Cinemas { get; set; }
    }
}