using TicketSystem.Core.Enum;

namespace TicketSystem.Application.DTOs.Ticket
{
    public class UpdateTicketDto
    {
        public TicketStatus? Status { get; set; }
        public Guid? AssignedToUserId { get; set; }
    }
}
