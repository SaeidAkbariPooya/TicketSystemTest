using TicketSystem.Core.Entities;

namespace TicketSystem.Core.IRepositories
{
    public interface ITicketRepository
    {
        Task<Ticket?> GetByIdAsync(Guid id);
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<IEnumerable<Ticket>> GetByUserIdAsync(Guid userId);
        Task AddAsync(Ticket ticket);
        Task UpdateAsync(Ticket ticket);
        Task DeleteAsync(Ticket ticket);
        Task<TicketStats> GetStatsAsync();
    }

    public record TicketStats
    {
        public int OpenCount { get; init; }
        public int InProgressCount { get; init; }
        public int ClosedCount { get; init; }
        public int TotalCount { get; init; }
    }
}
