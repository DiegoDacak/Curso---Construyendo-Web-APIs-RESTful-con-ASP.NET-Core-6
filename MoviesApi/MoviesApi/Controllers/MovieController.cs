using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesApi.Common.Messages;
using MoviesApi.Common.RoutesName;
using MoviesApi.Common.Strings;
using MoviesApi.Context;
using MoviesApi.DTOs.Movie;
using MoviesApi.Entities;
using MoviesApi.Services.HttpContextExtensions;
using MoviesApi.Services.Pagination;
using MoviesApi.Services.ServicesInterface;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MovieController : CustomBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;
        private readonly ILogger<MovieController> _logger;

        public MovieController(ApplicationDbContext context, IMapper mapper, IFileStorage fileStorage, 
            ILogger<MovieController> logger) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
            _fileStorage = fileStorage;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<MoviesIndexDto>> Get(CancellationToken token)
        {
            const int top = 5;
            var today = DateTime.Today;
            var futureRelease = await _context.Movies.Where(x => x.ReleaseDat > today)
                .OrderBy(x=>x.ReleaseDat).Take(top).ToListAsync(token);
            var atCinema = await _context.Movies.Where(x => x.AtCinema).Take(top).ToListAsync(token);

            var result = new MoviesIndexDto()
            {
                FutureRelease = _mapper.Map<List<MovieDto>>(futureRelease),
                AtCinema = _mapper.Map<List<MovieDto>>(atCinema)
            };
            return result;
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<MovieDto>>> Filter([FromQuery] FilterMovieDto filterMovieDto, 
            CancellationToken token)
        {
            var movieQueryable = _context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(filterMovieDto.Title))
            {
                movieQueryable = movieQueryable.Where(x => x.Title.Contains(filterMovieDto.Title));
            }

            if (filterMovieDto.AtCinema)
            {
                movieQueryable = movieQueryable.Where(x => x.AtCinema);
            }

            if (filterMovieDto.FutureRelease)
            {
                var today = DateTime.Today;
                movieQueryable = movieQueryable.Where(x => x.ReleaseDat > today);
            }

            if (filterMovieDto.GenderId != 0)
            {
                movieQueryable = movieQueryable
                    .Where(x => x.MovieGenders.Select(y => y.GenderId).Contains(filterMovieDto.GenderId));
            }

            if (!string.IsNullOrEmpty(filterMovieDto.Order))
            {
                var orderType = filterMovieDto.Ascending ? "ascending" : "descending";
                try
                {
                    movieQueryable = movieQueryable.OrderBy($"{filterMovieDto.Order} {orderType}");
                }
                catch (Exception error)
                {
                    _logger.LogError(error.Message, error);
                }
            }

            await HttpContext.SetPaginationParameters(movieQueryable, filterMovieDto.RegisterQuantityPerPage, token);

            var movies = await movieQueryable.Paginate(filterMovieDto.Pagination).ToListAsync(token);

            return _mapper.Map<List<MovieDto>>(movies);
        }

        [HttpGet("{id:int}", Name = RoutesName.GetMovieById)]
        public async Task<ActionResult<MovieDetailDto>> Get(int id, CancellationToken token)
        {
            var movie = await _context.Movies
                .Include(x => x.MovieActors).ThenInclude(x=>x.Actor)
                .Include(x=>x.MovieGenders).ThenInclude(x=>x.Gender)
                .FirstOrDefaultAsync(x => x.Id == id, token);
            if (movie is null)
            {
                return NotFound();
            }

            movie.MovieActors = movie.MovieActors.OrderBy(x => x.Order).ToList();
            return _mapper.Map<MovieDetailDto>(movie);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] CreateMovieDto createMovieDto, CancellationToken token)
        {
            var movieEntity = _mapper.Map<Movie>(createMovieDto);

            if (createMovieDto.Poster is not null)
            {
                await using var memoryStream = new MemoryStream();
                await createMovieDto.Poster.CopyToAsync(memoryStream, token);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(createMovieDto.Poster.FileName);
                movieEntity.Poster = await _fileStorage.SaveFile(content, extension,
                Container.MovieContainer, createMovieDto.Poster.ContentType);
            }
            AssignOrderMovies(movieEntity);
            _context.Add(movieEntity);
            await _context.SaveChangesAsync(token);
            var movieDto = _mapper.Map<MovieDto>(movieEntity);
            return new CreatedAtRouteResult(RoutesName.GetMovieById, new {id = movieEntity.Id}, movieDto);
        }

        private static void AssignOrderMovies(Movie movie)
        {
            if (movie.MovieActors == null) return;
            for (var i = 0; i < movie.MovieActors.Count; i++)
            {
                movie.MovieActors[i].Order = i;
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] CreateMovieDto createMovieDto, CancellationToken token)
        {
            var movieDb = await _context.Movies
                .Include(x=> x.MovieActors)
                .Include(x=> x.MovieGenders)
                .FirstOrDefaultAsync(x => x.Id == id, token);
    
            if (movieDb == null)
            {
                return NotFound();
            }

            movieDb = _mapper.Map(createMovieDto, movieDb);
            
            if (createMovieDto.Poster is not null)
            {
                await using var memoryStream = new MemoryStream();
                await createMovieDto.Poster.CopyToAsync(memoryStream, token);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(createMovieDto.Poster.FileName);
                movieDb.Poster = await _fileStorage.EditFile(content, extension,
                    Container.ActorContainer,movieDb.Poster, createMovieDto.Poster.ContentType);
            }
            AssignOrderMovies(movieDb);
            await _context.SaveChangesAsync(token);
            return NoContent();
        }
        
        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<MoviePatchDto> patchDocument, 
            CancellationToken token)
        {
            return await Patch<Movie, MoviePatchDto>(id, patchDocument, token);
        }
        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id, CancellationToken token)
        {
            return await Delete<Movie>(id,OkMessages.DeletedMovie, NotFoundMessages.MovieNotExist, token);
        }
    }
}