
namespace TicketSystem.Application.DTOs.Ticket
{
    public class TicketStatsDto
    {
        public int OpenCount { get; set; }
        public int InProgressCount { get; set; }
        public int ClosedCount { get; set; }
        public int TotalCount { get; set; }
    }
}
