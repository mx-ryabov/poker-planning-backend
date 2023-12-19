using PokerPlanning.Domain.src.Models.TicketAggregate;

namespace PokerPlanning.Domain.src.Models.GameAggregate.Entities;

public class VotingProcess
{
    public VotingProcess()
    { }

    public Ticket? Ticket { get; set; }
    public Guid? TicketId { get; set; }
    public bool IsActive { get; set; } = false;
}
