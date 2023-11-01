using PokerPlanning.Domain.src.BaseModels;

namespace PokerPlanning.Domain.src.Models.VotingSystemAggregate.Entities;

public class VotingSystemVote : Entity<Guid>
{
    public VotingSystemVote(Guid id) : base(id)
    {
    }

    public required string Value { get; set; }
    public required Guid VotingSystemId { get; set; }
}
