using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MoviesApi.DTOs.Actor;
using MoviesApi.DTOs.Cinema;
using MoviesApi.DTOs.Genre;
using MoviesApi.DTOs.Movie;
using MoviesApi.Entities;
using NetTopologySuite.Geometries;

namespace MoviesApi.Services.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile(GeometryFactory geometryFactory)
        {
            CreateMap<Gender, GenderDto>().ReverseMap();
            CreateMap<CreateGenderDto, Gender>();

            CreateMap<Cinema, CinemaDto>()
                .ForMember(x => x.Latitude, 
                    opt => 
                        opt.MapFrom(y => y.Location.Y))
                .ForMember(x => x.Longitude, 
                    opt => 
                        opt.MapFrom(y => y.Location.X));

            CreateMap<CinemaDto, Cinema>()
                .ForMember(x => x.Location, opt =>
                    opt.MapFrom(y => geometryFactory
                        .CreatePoint(new Coordinate(y.Latitude, y.Longitude))));
            
            CreateMap<CreateCinemaDto, Cinema>()
                .ForMember(x => x.Location, opt =>
                    opt.MapFrom(y => geometryFactory
                        .CreatePoint(new Coordinate(y.Latitude, y.Longitude))));;

            CreateMap<Actor, ActorDto>().ReverseMap();
            CreateMap<CreateActorDto, Actor>()
                .ForMember(x => x.Photo, 
                    opt => opt.Ignore());
            CreateMap<ActorPatchDto, Actor>().ReverseMap();
            
            CreateMap<Movie, MovieDto>().ReverseMap();
            
            CreateMap<CreateMovieDto, Movie>()
                .ForMember(x => x.Poster, 
                    opt => opt.Ignore())
                .ForMember(x=>x.MovieGenders, 
                    opt => opt.MapFrom(MapMoviesGenders))
                .ForMember(x=>x.MovieActors, 
                    opt => opt.MapFrom(MapMoviesActors));
            CreateMap<MoviePatchDto, Movie>().ReverseMap();
            CreateMap<Movie, MovieDetailDto>()
                .ForMember(x => 
                    x.Gender, opt => opt.MapFrom(MapMovieGender))
                .ForMember(x=>
                    x.Actor, opt => opt.MapFrom(MapMovieActor));
            
        }
        private static List<MovieActorDetailDto> MapMovieActor(Movie movie, MovieDetailDto movieDetailDto)
        {
            var result = new List<MovieActorDetailDto>();
            if (movie.MovieActors == null)
            {
                return result;
            }

            result.AddRange(movie.MovieActors.Select(movieActor => 
                new MovieActorDetailDto()
                {
                    ActorId = movieActor.ActorId,
                    Name = movieActor.Actor.Name, 
                    Character = movieActor.Character
                }));

            return result;
        }
        
        private static List<GenderDto> MapMovieGender(Movie movie, MovieDetailDto movieDetailDto)
        {
            var result = new List<GenderDto>();
            if (movie.MovieGenders == null)
            {
                return result;
            }

            result.AddRange(movie.MovieGenders.Select(movieGender => 
                new GenderDto() {Name = movieGender.Gender.Name}));

            return result;
        }
        private static IEnumerable<MovieGender> MapMoviesGenders(CreateMovieDto createMovieDto, Movie movie)
        {
            var result = new List<MovieGender>();
            if (createMovieDto.GendersId == null)
            {
                return result;
            }

            result.AddRange(createMovieDto.GendersId.Select(id => new MovieGender() {GenderId = id}));

            return result;
        }
        
        private static IEnumerable<MovieActor> MapMoviesActors(CreateMovieDto createMovieDto, Movie movie)
        {
            var result = new List<MovieActor>();
            if (createMovieDto.Actors == null)
            {
                return result;
            }

            result.AddRange(createMovieDto.Actors.Select(actor => new MovieActor() {ActorId = actor.ActorId, Character = actor.Character}));

            return result;
        }
    }
}