using Microsoft.AspNetCore.Identity;
using TicketSystem.Core.IServices;


namespace TicketSystem.Infra.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly PasswordHasher<object> _passwordHasher = new();

        public string HashPassword(string password)
            => _passwordHasher.HashPassword(null!, password);

        public bool VerifyPassword(string password, string passwordHash)
            => _passwordHasher.VerifyHashedPassword(null!, passwordHash, password)
                == PasswordVerificationResult.Success;
    }
}
