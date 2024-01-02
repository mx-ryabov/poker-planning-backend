using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Common;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.Domain.src.Models.TicketAggregate;
using PokerPlanning.Domain.src.Models.VotingSystemAggregate;

namespace PokerPlanning.Domain.src.Models.GameAggregate;

public class Game : AggregateRoot<Guid>
{
    protected Game(Guid id) : base(id)
    {
    }

    public required string Name { get; set; }
    public required string Link { get; set; }
    public required GameSettings Settings { get; set; }
    public required VotingProcess VotingProcess { get; set; }
    public VotingSystem VotingSystem { get; set; } = null!;
    public required Guid VotingSystemId { get; set; }
    public List<Participant> Participants { get; set; } = new List<Participant>();
    public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    public List<VotingResult> VotingResults { get; set; } = new List<VotingResult>();

    public static Game Create(string name, string link, GameSettings settings, Guid votingSystemId, Participant master)
    {
        var votingProcess = new VotingProcess();
        return new(Guid.NewGuid())
        {
            Name = name,
            Link = link,
            Settings = settings,
            VotingProcess = votingProcess,
            VotingSystemId = votingSystemId,
            Participants = new List<Participant>() { master }
        };
    }

    public UpdateResult ChangeVotingProcess(VotingProcess votingProcess, Guid changerUserId)
    {
        try
        {
            var participant = Participants.Single(p => p.UserId == changerUserId);
            var allowedRolesForChanging = new List<ParticipantRole>() { ParticipantRole.Master, ParticipantRole.Manager };
            if (!allowedRolesForChanging.Contains(participant.Role))
            {
                return new UpdateResult(
                    false,
                    new() { "This user isn't allowed to change the voting process." }
                );
            }
        }
        catch (Exception)
        {
            return new UpdateResult(
                false,
                new() { "There are no participants with this user id." }
            );
        }

        VotingProcess = votingProcess;
        return new UpdateResult(true);
    }

    public UpdateResult AddParticipant(Participant participant)
    {
        if (Participants.Count >= 10)
        {
            return new UpdateResult(
                false,
                new() { "The maximum number of participants has been exceeded." }
            );
        }

        Participants.Add(participant);
        return new UpdateResult(true);
    }
}
