using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Models.TicketAggregate;

namespace PokerPlanning.Domain.src.Models.GameAggregate.Entities;

public class VotingResult : Entity<Guid>
{
    protected VotingResult(Guid id) : base(id)
    {
    }

    public required Game Game { get; set; }
    public Ticket? Ticket { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public List<VotingResultVote> Votes { get; set; } = new List<VotingResultVote>();
}
