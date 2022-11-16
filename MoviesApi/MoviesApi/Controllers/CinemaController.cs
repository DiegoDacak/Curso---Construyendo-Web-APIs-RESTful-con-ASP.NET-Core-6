using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Common.Messages;
using MoviesApi.Common.RoutesName;
using MoviesApi.Context;
using MoviesApi.DTOs.Cinema;
using MoviesApi.Entities;
using NetTopologySuite.Geometries;

namespace MoviesApi.Controllers
{
    [Route("api/cinema")]
    [ApiController]
    public class CinemaController : CustomBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly GeometryFactory _geometryFactory;

        public CinemaController(ApplicationDbContext context, IMapper mapper, GeometryFactory geometryFactory) : 
            base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
            _geometryFactory = geometryFactory;
        }

        [HttpGet]
        public async Task<ActionResult<List<CinemaDto>>> Get(CancellationToken token)
        {
            return await Get<Cinema, CinemaDto>(token);
        }

        [HttpGet("{id:int}", Name = RoutesName.GetCinemaById)]
        public async Task<ActionResult<CinemaDto>> Get(int id, CancellationToken token)
        {
            return await GetById<Cinema, CinemaDto>(id, NotFoundMessages.CinemaNotExist, token);
        }

        [HttpGet("nearby")]
        public async Task<ActionResult<List<NearbyCinemaDto>>> Nearby([FromQuery] NearbyCinemaFilterDto filter, 
            CancellationToken token)
        {
            var userLocation = _geometryFactory.CreatePoint(new Coordinate(filter.Longitude, filter.Latitude));
            var cinema = await _context.Cinemas
                .OrderBy(x => x.Location.Distance(userLocation))
                .Where(x => x.Location.IsWithinDistance(userLocation, filter.DistanceInKm * 1000))
                .Select(x => new NearbyCinemaDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Latitude = x.Location.Y,
                    Longitude = x.Location.X,
                    DistanceInKm = Math.Round(x.Location.Distance(userLocation) * 1000)
                }).ToListAsync(token);
            return cinema;
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateCinemaDto createCinemaDto, CancellationToken token)
        {
            return await Post<Cinema, CreateCinemaDto>(createCinemaDto, RoutesName.GetMovieById, 
                OkMessages.CinemaCreated, token);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id, CancellationToken token)
        {
            return await Delete<Cinema>(id, OkMessages.DeletedCinema, NotFoundMessages.CinemaNotExist, token);
        }
    }
}