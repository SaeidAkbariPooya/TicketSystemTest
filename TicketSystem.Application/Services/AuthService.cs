using TicketSystem.Application.DTOs;
using TicketSystem.Application.IService;
using TicketSystem.Core.IRepositories;
using TicketSystem.Core.IServices;

namespace TicketSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, IPasswordHasherService passwordHasherService, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasherService = passwordHasherService;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !_passwordHasherService.VerifyPassword(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            var token = _tokenService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                ExpiresAt = DateTime.UtcNow.AddHours(2)
            };
        }
    }
}
