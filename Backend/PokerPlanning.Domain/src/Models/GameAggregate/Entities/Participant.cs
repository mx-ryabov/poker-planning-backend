using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.Domain.src.Models.UserAggregate;
using PokerPlanning.Domain.src.Models.VotingSystemAggregate.Entities;

namespace PokerPlanning.Domain.src.Models.GameAggregate.Entities;

public class Participant : Entity<Guid>
{
    protected Participant(Guid id) : base(id)
    {
    }

    public required string DisplayName { get; set; }
    public VotingSystemVote? Vote { get; set; }
    public User? User { get; set; } = null;
    public Game Game { get; set; } = null!;
    public Guid GameId { get; set; }
    public required ParticipantRole Role { get; set; }

    public static Participant Create(string displayName, ParticipantRole role, User? user)
    {
        return new(Guid.NewGuid())
        {
            DisplayName = displayName,
            Role = role,
            User = user
        };
    }
}
