using TicketSystem.Application.DTOs;

namespace TicketSystem.Application.IService
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto request);
    }
}
