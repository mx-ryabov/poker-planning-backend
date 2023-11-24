using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Models.VotingSystemAggregate.Entities;

namespace PokerPlanning.Domain.src.Models.GameAggregate.Entities;

public class VotingResultVote : Entity<Guid>
{
    protected VotingResultVote(Guid id) : base(id)
    {
    }

    public required VotingSystemVote Vote { get; set; }
    public required Participant Participant { get; set; }
}
