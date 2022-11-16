using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Common.Enum;
using MoviesApi.DTOs.Movie.Relations;
using MoviesApi.Services.ModelBinder;
using MoviesApi.Validations;

namespace MoviesApi.DTOs.Movie
{
    public class CreateMovieDto : MoviePatchDto
    {
        [ArchiveTypeValidation(archiveTypeGroup: ArchiveTypeGroup.Image)]
        [ArchiveSizeValidation(maxSizeMb: 4)]
        public IFormFile Poster { get; set; }
        
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GendersId { get; set; }
        
        [ModelBinder(BinderType = typeof(TypeBinder<List<MovieActorCreateDto>>))]
        public List<MovieActorCreateDto> Actors { get; set; }
    }
}