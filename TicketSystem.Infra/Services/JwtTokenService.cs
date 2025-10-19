using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicketSystem.Core.Entities;
using TicketSystem.Core.IServices;

namespace TicketSystem.Infra.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration) => _configuration = configuration;

        public class Constansts
        {
            public const int JWT_TOKEN_EXPIRE_TIME = 12;
            public const string JWT_SECURITY_KEY_FOR_TOKEN = "CODEING_DROPLETS_DADAD";
        }
        public string GenerateToken(User user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]
                ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong123!");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        }),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = _configuration["Jwt:Issuer"] ?? "TicketSystem",
                Audience = _configuration["Jwt:Audience"] ?? "TicketSystemUsers",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}