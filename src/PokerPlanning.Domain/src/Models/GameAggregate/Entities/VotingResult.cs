using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Models.TicketAggregate;

namespace PokerPlanning.Domain.src.Models.GameAggregate.Entities;

public class VotingResult : Entity<Guid>
{
    protected VotingResult(Guid id) : base(id)
    {
    }

    public Game Game { get; set; } = null!;
    public Guid GameId { get; set; }
    public Ticket? Ticket { get; set; }
    public Guid? TicketId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public List<VotingResultVote> Votes { get; set; } = new List<VotingResultVote>();

    public static VotingResult Create(Guid gameId, List<VotingResultVote> votes, Guid? ticketId)
    {
        return new(Guid.NewGuid())
        {
            GameId = gameId,
            Votes = votes,
            TicketId = ticketId,
        };
    }
}
