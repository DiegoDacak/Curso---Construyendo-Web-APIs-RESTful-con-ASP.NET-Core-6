using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIAutores.DTOs;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class RootController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;

        public RootController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }
        
        [HttpGet(Name = "obtenerRoot")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DatoHATEOAS>>> Get()
        {
            var esAdmin = await _authorizationService.AuthorizeAsync(User, "esAdmin");
            var datosHateoas = new List<DatoHATEOAS>();
            datosHateoas.Add(new DatoHATEOAS(
                enlace: Url.Link("obtenerRoot", new {}),
                descripcion:"self",
                metodo:"Get"));
            datosHateoas.Add(new DatoHATEOAS(
                enlace: Url.Link("obtenerAutores", new {}),
                descripcion:"autores",
                metodo:"Get"));
            if (esAdmin.Succeeded)
            {
                datosHateoas.Add(new DatoHATEOAS(
                    enlace: Url.Link("crearAutor", new {}),
                    descripcion:"autores",
                    metodo:"Get"));
                datosHateoas.Add(new DatoHATEOAS(
                    enlace: Url.Link("crearLibro", new {}),
                    descripcion:"autores",
                    metodo:"Get"));
            }
            return datosHateoas;
        }
    }
}