using TicketSystem.Application.DTOs.Ticket;

namespace TicketSystem.Application.IService
{
    public interface ITicketService
    {
        Task<TicketResponseDto> CreateTicketAsync(CreateTicketDto request, Guid userId);
        Task<IEnumerable<TicketResponseDto>> GetMyTicketsAsync(Guid userId);
        Task<IEnumerable<TicketResponseDto>> GetAllTicketsAsync();
        Task<TicketResponseDto> UpdateTicketAsync(Guid ticketId, UpdateTicketDto request);
        Task<TicketStatsDto> GetStatsAsync();
        Task<TicketResponseDto> GetTicketByIdAsync(Guid ticketId, Guid userId, string userRole);
        Task DeleteTicketAsync(Guid ticketId);
    }
}
