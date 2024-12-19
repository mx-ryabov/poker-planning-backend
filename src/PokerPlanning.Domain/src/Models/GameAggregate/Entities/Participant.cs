using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Common.Results;
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
    public required bool Online { get; set; }
    public VotingSystemVote? Vote { get; set; }
    public Guid? VoteId { get; set; }
    public User? User { get; set; } = null;
    public Guid? UserId { get; set; } = null;
    public Game Game { get; set; } = null!;
    public Guid GameId { get; set; }
    public required ParticipantRole Role { get; set; }

    public static Participant Create(string displayName, ParticipantRole role, User? user)
    {
        return new(Guid.NewGuid())
        {
            DisplayName = displayName,
            Role = role,
            User = user,
            Online = false
        };
    }

    public UpdateResult DoVote(Guid? voteId)
    {
        if (!Game.VotingProcess.IsActive)
        {
            return UpdateResult.Error(
                new() { "The voting process is inactive." }
            );
        }
        VoteId = voteId;
        return UpdateResult.Ok();
    }

    public UpdateResult GoOnline()
    {
        Online = true;
        return UpdateResult.Ok();
    }

    public UpdateResult GoOffline()
    {
        Online = false;
        return UpdateResult.Ok();
    }
}
