using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.Domain.src.Models.UserAggregate;
using PokerPlanning.Domain.src.Models.VotingSystemAggregate.Entities;

namespace PokerPlanning.Domain.src.Models.GameAggregate.Entities;

public class Participant : Entity<Guid>
{
    public Participant(Guid id) : base(id)
    {
    }

    public required string DisplayName { get; set; }
    public VotingSystemVote? Vote { get; set; }
    public User? User { get; set; } = null;
    public required Game Game { get; set; }
    public required ParticipantRole Role { get; set; }
}
