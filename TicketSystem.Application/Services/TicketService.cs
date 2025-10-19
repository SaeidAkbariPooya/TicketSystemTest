using TicketSystem.Application.DTOs.Ticket;
using TicketSystem.Application.IService;
using TicketSystem.Core.Entities;
using TicketSystem.Core.Enum;
using TicketSystem.Core.IRepositories;

namespace TicketSystem.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;

        public TicketService(ITicketRepository ticketRepository, IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
        }

        public async Task<TicketResponseDto> CreateTicketAsync(CreateTicketDto request, Guid userId)
        {
            var ticket = new Ticket
            {
                Title = request.Title,
                Description = request.Description,
                Priority = request.Priority,
                CreatedByUserId = userId
            };

            await _ticketRepository.AddAsync(ticket);
            var createdTicket = await _ticketRepository.GetByIdAsync(ticket.Id);

            return MapToDto(createdTicket!);
        }

        public async Task<IEnumerable<TicketResponseDto>> GetMyTicketsAsync(Guid userId)
        {
            var tickets = await _ticketRepository.GetByUserIdAsync(userId);
            return tickets.Select(MapToDto);
        }

        public async Task<IEnumerable<TicketResponseDto>> GetAllTicketsAsync()
        {
            var tickets = await _ticketRepository.GetAllAsync();
            return tickets.Select(MapToDto);
        }

        public async Task<TicketResponseDto> UpdateTicketAsync(Guid ticketId, UpdateTicketDto request)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null)
                throw new KeyNotFoundException("Ticket not found");

            if (request.Status.HasValue)
                ticket.Status = request.Status.Value;

            if (request.AssignedToUserId.HasValue)
            {
                var admin = await _userRepository.GetByIdAsync(request.AssignedToUserId.Value);
                if (admin == null || admin.Role != UserRole.Admin)
                    throw new ArgumentException("Invalid admin user ID");

                ticket.AssignedToUserId = request.AssignedToUserId.Value;
            }

            await _ticketRepository.UpdateAsync(ticket);
            var updatedTicket = await _ticketRepository.GetByIdAsync(ticketId);

            return MapToDto(updatedTicket!);
        }

        public async Task<TicketStatsDto> GetStatsAsync()
        {
            var stats = await _ticketRepository.GetStatsAsync();
            return new TicketStatsDto
            {
                OpenCount = stats.OpenCount,
                InProgressCount = stats.InProgressCount,
                ClosedCount = stats.ClosedCount,
                TotalCount = stats.TotalCount
            };
        }

        public async Task<TicketResponseDto> GetTicketByIdAsync(Guid ticketId, Guid userId, string userRole)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null)
                throw new KeyNotFoundException("Ticket not found");

            if (userRole != "Admin" && ticket.CreatedByUserId != userId)
                throw new UnauthorizedAccessException("Access denied");

            return MapToDto(ticket);
        }

        public async Task DeleteTicketAsync(Guid ticketId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null)
                throw new KeyNotFoundException("Ticket not found");

            await _ticketRepository.DeleteAsync(ticket);
        }

        private static TicketResponseDto MapToDto(Ticket ticket) => new()
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            Status = ticket.Status.ToString(),
            Priority = ticket.Priority.ToString(),
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt,
            CreatedByUserName = ticket.CreatedByUser.FullName,
            AssignedToUserName = ticket.AssignedToUser?.FullName
        };
    }
}