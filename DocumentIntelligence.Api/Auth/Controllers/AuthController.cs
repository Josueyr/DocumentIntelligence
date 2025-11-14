using DocumentIntelligence.Api.Auth.Interfaces;
using DocumentIntelligence.Api.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentIntelligence.Api.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Nota: esto es una demo. Sustituir por validación real de usuarios.
            if (string.IsNullOrEmpty(request.Username))
                return BadRequest("Invalid username");

            var token = _tokenService.GenerateToken(request.Username);
            return Ok(new { token });
        }
    }

    public record LoginRequest(string Username);
}