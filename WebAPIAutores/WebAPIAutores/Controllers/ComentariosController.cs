using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public ComentariosController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("{id:int}", Name = "obtenerComentario")]
        public async Task<ActionResult<ComentarioDTO>> GetById(int id)
        {
            var comentario = await _context.Comentarios.FirstOrDefaultAsync(x => x.Id == id);
            if (comentario is null)
            {
                return NotFound();
            }

            return _mapper.Map<ComentarioDTO>(comentario);
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroId)
        {
            var existeLibro = await _context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
            {
                return NotFound();
            }
            var comentarios = await _context.Comentarios.Where(x => x.Id == libroId).ToListAsync();
            return _mapper.Map<List<ComentarioDTO>>(comentarios);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreacionDto)
        {
            var emailClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email");
            if (emailClaim != null)
            {
                var usuario = _userManager.FindByEmailAsync(emailClaim.Value);
                var usuarioId = usuario.Id;
            }
            var existeLibro = await _context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
            {
                return NotFound();
            }

            var comentario = _mapper.Map<Comentario>(comentarioCreacionDto);
            comentario.LibroId = libroId;
            _context.Add((object) comentario);
            await _context.SaveChangesAsync();
            var comentarioDTO = _mapper.Map<ComentarioDTO>(comentario);
            return CreatedAtRoute("otenerComentario", new {id = comentario.Id, libroId = libroId}, comentarioDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int libroId, int id, ComentarioCreacionDTO comentarioCreacionDto)
        {
            var existeLibro = await _context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
            {
                return NotFound();
            }

            var existeComentario = await _context.Comentarios.AnyAsync(x => x.Id == id);
            if (!existeComentario)
            {
                return NotFound();
            }
            
            var comentario = _mapper.Map<Comentario>(comentarioCreacionDto);
            comentario.Id = id;
            comentario.LibroId = libroId;
            _context.Update(comentario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
    }
}