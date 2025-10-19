
namespace TicketSystem.Application.DTOs.Ticket
{
    public class TicketResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedByUserName { get; set; } = string.Empty;
        public string? AssignedToUserName { get; set; }
    }
}
