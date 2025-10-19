using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Application.DTOs;
using TicketSystem.Application.IService;

namespace TicketSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginDto), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for {Email}", request.Email);
                return StatusCode(500, new { message = "An error occurred during login" });
            }
        }
    }
}
