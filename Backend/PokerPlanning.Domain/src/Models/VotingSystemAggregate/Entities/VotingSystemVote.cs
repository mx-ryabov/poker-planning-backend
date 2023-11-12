using PokerPlanning.Domain.src.BaseModels;

namespace PokerPlanning.Domain.src.Models.VotingSystemAggregate.Entities;

public class VotingSystemVote : Entity<Guid>
{
    protected VotingSystemVote(Guid id) : base(id)
    {
    }

    public required string Value { get; set; }
    public required decimal Order { get; set; }
    public required string Suit { get; set; }
    public required Guid VotingSystemId { get; set; }

    public static VotingSystemVote Create(string value, decimal order, string suit, Guid votingSystemId)
    {
        return new(Guid.NewGuid()) { Value = value, Order = order, Suit = suit, VotingSystemId = votingSystemId };
    }
}
