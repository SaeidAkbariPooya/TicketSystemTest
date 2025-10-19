using TicketSystem.Core.Entities;

namespace TicketSystem.Core.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task<bool> ExistsAsync(string email);
    }
}
