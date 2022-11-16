using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        public readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("id:{Id}",Name = "obtenerLibro")]
        public async Task<ActionResult<LibroDTConAutores>> Get(int Id)
        {
            var libros = await _context.Libros
                .Include(libroDB=>libroDB.AutoresLibros)
                .ThenInclude(autorLibroDB => autorLibroDB.Autor)
                .FirstOrDefaultAsync(x => x.Id == Id);
            if (libros is null)
            {
                return BadRequest();
            }
            libros.AutoresLibros = libros.AutoresLibros.OrderBy(x => x.Orden).ToList();
            return _mapper.Map<LibroDTConAutores>(libros);
        }

        [HttpPost(Name = "crearLibro")]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDto)
        {
            if (libroCreacionDto.AutoresIds == null)
            {
                return BadRequest("No se puede crear un libro sin autores");
            }
        
            var autoresIds = await _context.Autores
                .Where(autorDb => libroCreacionDto.AutoresIds.Contains(autorDb.Id)).Select(x => x.Id).ToListAsync();
            if (libroCreacionDto.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest("No existe uno de los autores enviados");
            }
        
            var libro = _mapper.Map<Libro>(libroCreacionDto);
            AsignarOrdenAutores(libro);
        
            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();
            var libroDTO = _mapper.Map<LibroDTO>(libro);
            return CreatedAtRoute("obtenerLibro", new{ id = libro.Id }, libroDTO);
        }

        [HttpPatch("{id:int}", Name = "patchLibro")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> patchDocument)
        {
            if (patchDocument is null)
            {
                return BadRequest();
            }

            var libroDB = await _context.Libros.FirstOrDefaultAsync(x => x.Id == id);
            if (libroDB is null)
            {
                return NotFound();
            }

            var libroDTO = _mapper.Map<LibroPatchDTO>(libroDB);
            
            patchDocument.ApplyTo(libroDTO, ModelState);

            var esValido = TryValidateModel(libroDTO);
            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(libroDTO, libroDB);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete(Name = "borrarLibro")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await _context.Libros.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return BadRequest("Autor no encontrado");
            }
        
            _context.Remove(new Libro() {Id = id});
            await _context.SaveChangesAsync();
            return Ok("Libro eliminado");
        }
        
        private void AsignarOrdenAutores(Libro libro)
        {
            if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }
            }
        }
    }
}