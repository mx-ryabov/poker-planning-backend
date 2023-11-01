using PokerPlanning.Domain.src.BaseModels;

namespace PokerPlanning.Domain.src.Models.GameAggregate.Entities;

public class VotingResultVote : Entity<Guid>
{
    public VotingResultVote(Guid id) : base(id)
    {
    }


}
