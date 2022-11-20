using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Context;
using MoviesApi.DTOs.Pagination;
using MoviesApi.Entities.Interfaces;
using MoviesApi.Services.HttpContextExtensions;
using MoviesApi.Services.Pagination;

namespace MoviesApi.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CustomBaseController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        protected async Task<List<TDto>> Get<TEntity, TDto>(CancellationToken token) where TEntity : class
        {
            var entities = await _context.Set<TEntity>().AsNoTracking().ToListAsync(token);
            var result = _mapper.Map<List<TDto>>(entities);
            return result;
        }

        protected async Task<List<TDto>> GetPagination<TEntity, TDto>(PaginationDto paginationDto, 
            CancellationToken token) where TEntity : class
        {
            var queryable = _context.Set<TEntity>().AsQueryable();
            await HttpContext.SetPaginationParameters(queryable, paginationDto.RegisterQuantityPerPage, token);
            var entities = await queryable.Paginate(paginationDto).ToListAsync(token);
            return _mapper.Map<List<TDto>>(entities);
        }

        protected async Task<ActionResult<TDto>> GetById<TEntity, TDto>(int id, string badRequestMessage, 
            CancellationToken token) where TEntity : class, IId
        {
            var entity = await _context.Set<TEntity>().AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, token);
            if (entity is null)
            {
                return NotFound(badRequestMessage);
            }
            return _mapper.Map<TDto>(entity);
        }

        protected async Task<ActionResult> Post<TEntity, TDto>(TDto dto, string routeName, string okMessage,
            CancellationToken token) where TEntity : class, IId
        {
            var newEntity = _mapper.Map<TEntity>(dto);
            _context.Add(newEntity);
            await _context.SaveChangesAsync(token);
            return new CreatedAtRouteResult(
                routeName, new {id = newEntity.Id }, okMessage);
        }

        protected async Task<ActionResult> Put<TEntity, TDto>(int id, TDto dto, string okMessage,
            CancellationToken token) where TEntity : class, IId
        {
            var entity = _mapper.Map<TEntity>(dto);
            entity.Id = id;
            // In this form we advice entity framework that the entity is modified
            // _context.Entry(entity).State = EntityState.Modified;
            _context.Update(entity);
            await _context.SaveChangesAsync(token);
            return Ok(okMessage);
        }

        protected async Task<ActionResult> Patch<TEntity, TDto>(int id, JsonPatchDocument<TDto> patchDocument,
            CancellationToken token) 
            where TEntity : class, IId
            where TDto: class
        {
            if (patchDocument is null)
            {
                return BadRequest();
            }

            var entityDb = await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id, token);
            if (entityDb is null)
            {
                return NotFound();
            }

            var entityDto = _mapper.Map<TDto>(entityDb);
            
            patchDocument.ApplyTo(entityDto, ModelState);

            var isValid = TryValidateModel(entityDto);
            
            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(entityDto, entityDb);

            await _context.SaveChangesAsync(token);

            return NoContent();
        }

        protected async Task<ActionResult> Delete<TEntity>(int id, string okMessage, string notFoundMessage,
            CancellationToken token) where TEntity : class, IId, new()
        {
            var exist = await _context.Set<TEntity>().AnyAsync(x=>x.Id == id, token);
            if (!exist)
            {
                return NotFound(notFoundMessage);
            }
            _context.Remove(new TEntity() {Id = id});
            await _context.SaveChangesAsync(token);
            return Ok(okMessage);
        }
    }
}