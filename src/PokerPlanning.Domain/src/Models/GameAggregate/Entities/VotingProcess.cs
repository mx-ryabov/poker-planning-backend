using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.Domain.src.Models.TicketAggregate;

namespace PokerPlanning.Domain.src.Models.GameAggregate.Entities;

public class VotingProcess
{
    public VotingProcess()
    { }

    public Ticket? Ticket { get; set; }
    public Guid? TicketId { get; set; }
    public VotingStatus Status { get; set; } = VotingStatus.Inactive;
}
