
namespace TicketSystem.Core.IServices
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
