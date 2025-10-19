using Microsoft.EntityFrameworkCore;
using TicketSystem.Core.Entities;
using TicketSystem.Core.Enum;
using TicketSystem.Core.IRepositories;
using TicketSystem.Infra.Context;

namespace TicketSystem.Infra.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketSystemDbContext _context;

        public TicketRepository(TicketSystemDbContext context) => _context = context;

        public async Task<Ticket?> GetByIdAsync(Guid id)
               => await _context.Tickets
                   .Include(t => t.CreatedByUser)
                   .Include(t => t.AssignedToUser)
                   .FirstOrDefaultAsync(t => t.Id == id);

        public async Task<IEnumerable<Ticket>> GetAllAsync()
            => await _context.Tickets
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedToUser)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Ticket>> GetByUserIdAsync(Guid userId)
            => await _context.Tickets
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedToUser)
                .Where(t => t.CreatedByUserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

        public async Task AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            ticket.UpdatedAt = DateTime.UtcNow;
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Ticket ticket)
        {
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<TicketStats> GetStatsAsync()
        {
            var tickets = await _context.Tickets.ToListAsync();

            return new TicketStats
            {
                OpenCount = tickets.Count(t => t.Status == TicketStatus.Open),
                InProgressCount = tickets.Count(t => t.Status == TicketStatus.InProgress),
                ClosedCount = tickets.Count(t => t.Status == TicketStatus.Closed),
                TotalCount = tickets.Count
            };
        }
    }
}