using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebAPIAutores.DTOs;
using WebAPIAutores.Servicios;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly HashService _hashService;
        private readonly IDataProtector _dataProtector;


        public UserController(UserManager<IdentityUser> userManager, IConfiguration configuration,
            SignInManager<IdentityUser> signInManager, IDataProtectionProvider dataProtectionProvider,
            HashService hashService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _hashService = hashService;
            _dataProtector = dataProtectionProvider.CreateProtector("valor_unico_y_quizas_Secreto");
        }

        [HttpPost("registrar", Name =  "registrarUsuario")]

    public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialesUsuario credencialesUsuario)
        {
            var usuario = new IdentityUser
            {
                UserName = credencialesUsuario.Email,
                Email = credencialesUsuario.Email
            };
            var resultado = await _userManager.CreateAsync(usuario, credencialesUsuario.Password);
            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialesUsuario);
            }
            return BadRequest(resultado.Errors);
        }

        [HttpPost("login",Name = "loginUsuario")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login(CredencialesUsuario credencialesUsuario)
        {
            var resultado = await _signInManager.PasswordSignInAsync(credencialesUsuario.Email,
                credencialesUsuario.Password, isPersistent: false, lockoutOnFailure: false);
            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialesUsuario);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }
        }

        [HttpGet("RenovarToken",Name = "renovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RespuestaAutenticacion>> Renovar()
        {
            var emailClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email");
            if (emailClaim != null)
            {
                var email = emailClaim.Value;
                var credencialesUsuario = new CredencialesUsuario()
                {
                    Email = email
                };
                return await ConstruirToken(credencialesUsuario);
            }
            return Unauthorized();
        }

        private async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credencialesUsuario)
        {
            var claims = new List<Claim>
            {
                new Claim("email", credencialesUsuario.Email)
            };
            var usuario = await _userManager.FindByEmailAsync(credencialesUsuario.Email);
            var claimsDb = await _userManager.GetClaimsAsync(usuario);
            
            claims.AddRange(claimsDb);
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["keyJwt"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var exp = DateTime.UtcNow.AddYears(1);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: exp,
                signingCredentials: creds);
            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = exp
            };
        }

        [HttpPost("HacerAdmin",Name = "hacerAdmin")]
        public async Task<ActionResult> HacerAdmin(EditarAdminDTO editarAdminDto)
        {
            var usuario = await _userManager.FindByEmailAsync(editarAdminDto.Email);
            await _userManager.AddClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }
        
        [HttpPost("RemoverAdmin",Name = "removerAdmin")]
        public async Task<ActionResult> RemoverAdmin(EditarAdminDTO editarAdminDto)
        {
            var usuario = await _userManager.FindByEmailAsync(editarAdminDto.Email);
            await _userManager.RemoveClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }
    }
}