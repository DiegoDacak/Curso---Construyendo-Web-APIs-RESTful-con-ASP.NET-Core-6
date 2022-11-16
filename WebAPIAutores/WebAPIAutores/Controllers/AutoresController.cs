using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class AutoresController : ControllerBase
    {
        public ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AutoresController(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet("configuration")]
        public ActionResult<string> Configuration()
        {
            return _configuration["apellido"];
        }

        [HttpGet(Name = "obtenerAutores")]
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {
            var autores = await _context.Autores.ToListAsync();
            return _mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpGet("{id:int}",Name = "obtenerAutor")]
        public async Task<ActionResult<AutorDTOConLibros>> Get(int id)
        {
            var autor = await _context.Autores
                .Include(x=>x.AutoresLibros)
                .ThenInclude(x=>x.Libro)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (autor == null)
            {
                return BadRequest();
            }
            
            var dto = _mapper.Map<AutorDTOConLibros>(autor);
            GenerarEnlaces(dto);
            return dto;
        }


        [HttpGet("{nombre}", Name = "obtenerAutorPorNombre")]
        public async Task<ActionResult<ColeccionDeRecursos<AutorDTO>>> Get([FromRoute] string nombre)
        {
            var autores = await _context.Autores.Where(x => x.Nombre.Contains(nombre)).ToListAsync();
            var dto = _mapper.Map<List<AutorDTO>>(autores);
            dto.ForEach(dto=>GenerarEnlaces(dto));
            var resultado = new ColeccionDeRecursos<AutorDTO> {Valores = dto};
            resultado.Enlace.Add(new DatoHATEOAS(
                enlace: Url.Link("obtenerAutores", new {}),
                descripcion: "Self",
                metodo: "Get"));
            resultado.Enlace.Add(new DatoHATEOAS(
                enlace: Url.Link("crearAutor", new {}),
                descripcion: "Self",
                metodo: "Post"));
            return resultado;
        } 

        [HttpPost(Name = "crearAutor")]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDto)
        {
            var existe = await _context.Autores.AnyAsync(x => x.Nombre == autorCreacionDto.Nombre);
            if (existe)
            {
                return BadRequest($"Ya existe el autor con el nombre {autorCreacionDto.Nombre}");
            }

            var autor = _mapper.Map<Autor>(autorCreacionDto);
            _context.Autores.Add(autor);
            await _context.SaveChangesAsync();
            var autorDTO = _mapper.Map<AutorDTO>(autor);
            return CreatedAtRoute("obtenerAutor", new{id=autor.Id}, autorDTO);
        }

        [HttpPut("{id:int}",Name = "actualizarAutor")]
        public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDto, int Id)
        {
            bool existe = await _context.Autores.AnyAsync(x => x.Id == Id);
            if (!existe)
            {
                return NotFound("Autor no encontrado");
            }

            var autor = _mapper.Map<Autor>(autorCreacionDto);
            _context.Autores.Update(autor);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete(Name = "borrarAutor")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await _context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return BadRequest("Autor no encontrado");
            }

            _context.Remove(new Autor() {Id = id});
            await _context.SaveChangesAsync();
            return Ok("Autor eliminado");
        }

        private void GenerarEnlaces(AutorDTO autorDto)
        {
            autorDto.Enlace.Add(new DatoHATEOAS(
                enlace: Url.Link("obtenerAutor", new {id = autorDto.Id}),
                descripcion: "Self",
                metodo: "Get"));
            autorDto.Enlace.Add(new DatoHATEOAS(
                enlace: Url.Link("actualizarAutor", new {id = autorDto.Id}),
                descripcion: "Self",
                metodo: "Put"));
        }
    }
}