using PokerPlanning.Domain.src.BaseModels;

namespace PokerPlanning.Domain.src.Models.GameAggregate.Entities;

public class VotingResult : Entity<Guid>
{
    protected VotingResult(Guid id) : base(id)
    {
    }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
