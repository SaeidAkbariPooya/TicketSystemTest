using TicketSystem.Core.Entities;

namespace TicketSystem.Core.IServices
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
