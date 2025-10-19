using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketSystem.Application.DTOs.Ticket;
using TicketSystem.Application.IService;

namespace TicketSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(ITicketService ticketService, ILogger<TicketsController> logger)
        {
            _ticketService = ticketService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(typeof(TicketResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<TicketResponseDto>> CreateTicket(CreateTicketDto request)
        {
            try
            {
                var userId = GetUserId();
                var ticket = await _ticketService.CreateTicketAsync(request, userId);
                return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ticket");
                return StatusCode(500, new { message = "An error occurred while creating ticket" });
            }
        }

        [HttpGet("GetMyTicket")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(typeof(IEnumerable<TicketResponseDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<IEnumerable<TicketResponseDto>>> GetMyTickets()
        {
            try
            {
                var userId = GetUserId();
                var tickets = await _ticketService.GetMyTicketsAsync(userId);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user tickets for {UserId}", GetUserId());
                return StatusCode(500, new { message = "An error occurred while getting tickets" });
            }
        }

        [HttpGet("GetAllTickets")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<TicketResponseDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<IEnumerable<TicketResponseDto>>> GetAllTickets()
        {
            try
            {
                var tickets = await _ticketService.GetAllTicketsAsync();
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all tickets");
                return StatusCode(500, new { message = "An error occurred while getting tickets" });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(TicketResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TicketResponseDto>> UpdateTicket(Guid id, UpdateTicketDto request)
        {
            try
            {
                var ticket = await _ticketService.UpdateTicketAsync(id, request);
                return Ok(ticket);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ticket {TicketId}", id);
                return StatusCode(500, new { message = "An error occurred while updating ticket" });
            }
        }

        [HttpGet("stats")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(TicketStatsDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<TicketStatsDto>> GetStats()
        {
            try
            {
                var stats = await _ticketService.GetStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ticket stats");
                return StatusCode(500, new { message = "An error occurred while getting stats" });
            }
        }


        //دریافت جزئیات یک تیکت خاص (مجاز به ایجادکننده و مدیر اختصاص داده
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketResponseDto>> GetTicket(Guid id)
        {
            try
            {
                var userId = GetUserId();
                var userRole = GetUserRole();
                var ticket = await _ticketService.GetTicketByIdAsync(id, userId, userRole);
                return Ok(ticket);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ticket {TicketId}", id);
                return StatusCode(500, new { message = "An error occurred while getting ticket" });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTicket(Guid id)
        {
            try
            {
                await _ticketService.DeleteTicketAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ticket {TicketId}", id);
                return StatusCode(500, new { message = "An error occurred while deleting ticket" });
            }
        }

        private Guid GetUserId()
            => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private string GetUserRole()
            => User.FindFirstValue(ClaimTypes.Role)!;
    }
}
