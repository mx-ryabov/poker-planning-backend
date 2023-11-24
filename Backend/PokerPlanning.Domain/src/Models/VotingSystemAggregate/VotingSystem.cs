using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Models.UserAggregate;
using PokerPlanning.Domain.src.Models.VotingSystemAggregate.Entities;

namespace PokerPlanning.Domain.src.Models.VotingSystemAggregate;

public class VotingSystem : AggregateRoot<Guid>
{
    protected VotingSystem(Guid id) : base(id)
    {
    }

    public required string Name { get; set; }
    public List<VotingSystemVote> Votes { get; set; } = new List<VotingSystemVote>();
    public User? Creator { get; set; } = null;

    public static VotingSystem Create(string name, Guid? id = null, User? creator = null)
    {
        return new(id ?? Guid.NewGuid())
        {
            Name = name,
            Creator = creator
        };
    }
}
