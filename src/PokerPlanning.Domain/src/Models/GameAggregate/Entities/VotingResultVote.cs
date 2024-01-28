using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Models.VotingSystemAggregate.Entities;

namespace PokerPlanning.Domain.src.Models.GameAggregate.Entities;

public class VotingResultVote : Entity<Guid>
{
    protected VotingResultVote(Guid id) : base(id)
    {
    }

    public VotingSystemVote? Vote { get; set; }
    public Guid? VoteId { get; set; }
    public Participant Participant { get; set; } = null!;
    public Guid ParticipantId { get; set; }

    public static VotingResultVote Create(Guid participantId, Guid? voteId)
    {
        return new(Guid.NewGuid())
        {
            VoteId = voteId,
            ParticipantId = participantId
        };
    }
}
