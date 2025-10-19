using TicketSystem.Core.Enum;

namespace TicketSystem.Core.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketStatus Status { get; set; } = TicketStatus.Open;
        public TicketPriority Priority { get; set; } = TicketPriority.Medium;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Guid CreatedByUserId { get; set; }
        public Guid? AssignedToUserId { get; set; }

        public User CreatedByUser { get; set; } = null!;
        public User? AssignedToUser { get; set; }
    }
}
