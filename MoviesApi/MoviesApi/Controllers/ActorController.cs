using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Common.Messages;
using MoviesApi.Common.RoutesName;
using MoviesApi.Common.Strings;
using MoviesApi.Context;
using MoviesApi.DTOs.Actor;
using MoviesApi.DTOs.Pagination;
using MoviesApi.Entities;
using MoviesApi.Services.ServicesInterface;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/actor")]
    public class ActorController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IFileStorage _fileStorage;
        
        public ActorController(IMapper mapper, ApplicationDbContext context, IFileStorage fileStorage) : base(context, mapper)
        {
            _mapper = mapper;
            _context = context;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDto>>> Get([FromQuery] PaginationDto paginationDto,CancellationToken token)
        {
            return await GetPagination<Actor, ActorDto>(paginationDto, token);
        }

        [HttpGet("{id:int}", Name = RoutesName.GetActorById)]
        public async Task<ActionResult<ActorDto>> Get(int id, CancellationToken token)
        {
            return await GetById<Actor, ActorDto>(id, NotFoundMessages.ActorNotExist, token);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] CreateActorDto createActorDto, CancellationToken token)
        {
            var actorEntity = _mapper.Map<Actor>(createActorDto);
            if (createActorDto.Photo is not null)
            {
                await using var memoryStream = new MemoryStream();
                await createActorDto.Photo.CopyToAsync(memoryStream, token);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(createActorDto.Photo.FileName);
                actorEntity.Photo = await _fileStorage.SaveArchive(content, extension,
                    Container.ActorContainer, createActorDto.Photo.ContentType);
            }
            _context.Add(actorEntity);
            await _context.SaveChangesAsync(token);
            var dto = _mapper.Map<ActorDto>(actorEntity);
            return new CreatedAtRouteResult(RoutesName.GetActorById, new {id = actorEntity.Id}, dto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] CreateActorDto createActorDto, CancellationToken token)
        {
            var actorDb = await _context.Actors.FirstOrDefaultAsync(x => x.Id == id, token);

            if (actorDb == null)
            {
                return NotFound(NotFoundMessages.ActorNotExist);
            }
            
            if (createActorDto.Photo is not null)
            {
                await using var memoryStream = new MemoryStream();
                await createActorDto.Photo.CopyToAsync(memoryStream, token);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(createActorDto.Photo.FileName);
                actorDb.Photo = await _fileStorage.EditArchive(content, extension,
                    Container.ActorContainer,actorDb.Photo, createActorDto.Photo.ContentType);
            }

            await _context.SaveChangesAsync(token);
            return Ok(OkMessages.ActorModify);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<ActorPatchDto> patchDocument, 
            CancellationToken token)
        {
            return await Patch<Actor, ActorPatchDto>(id, patchDocument, token);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id, CancellationToken token)
        {
            return await Delete<Actor>(id, OkMessages.DeletedActor, NotFoundMessages.ActorNotExist, token);
        }
    }
}