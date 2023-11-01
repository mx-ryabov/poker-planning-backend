using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.VotingSystemAggregate;

namespace PokerPlanning.Domain.src.Models.GameAggregate;

public class Game : AggregateRoot<Guid>
{
    protected Game(Guid id) : base(id)
    {
    }

    public required string Name { get; set; }
    public required string Link { get; set; }
    public bool IsAutoRevealCards { get; set; } = true;
    public List<Participant> Participants { get; set; } = new List<Participant>();
    public required VotingSystem VotingSystem { get; set; }
    public List<VotingResult> VotingResults { get; set; } = new List<VotingResult>();
}
