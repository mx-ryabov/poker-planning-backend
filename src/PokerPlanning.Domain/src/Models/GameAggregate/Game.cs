using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Common;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.Domain.src.Models.GameAggregate.Events;
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
        var game = new Game(Guid.NewGuid())
        {
            Name = name,
            Link = link,
            Settings = settings,
            VotingProcess = new(),
            VotingSystemId = votingSystemId,
            Participants = new() { master }
        };
        master.Game = game;
        return game;
    }

    public UpdateResult StartVotingProcess(Participant initiator, Guid? ticketId)
    {
        var canStart = IsParticipantCanChangeVotingProcess(initiator);
        if (!canStart)
        {
            return UpdateResult.Error(
                new() { "This participant isn't allowed to start the voting process." }
            );
        }

        VotingProcess.IsActive = true;
        VotingProcess.TicketId = ticketId;
        return UpdateResult.Ok();
    }

    public UpdateResult FinishVotingProcess(Participant initiator)
    {
        var canStart = IsParticipantCanChangeVotingProcess(initiator);
        if (!canStart)
        {
            return UpdateResult.Error(
                new() { "This participant isn't allowed to finish the voting process." }
            );
        }

        var votes = new List<VotingResultVote>();
        foreach (Participant p in Participants)
        {
            votes.Add(VotingResultVote.Create(p.Id, p.VoteId));
            p.DoVote(null);
        }
        var votingResult = VotingResult.Create(Id, votes, VotingProcess.TicketId);
        AddDomainEvent(new VotingProcessFinishedEvent(votingResult));

        VotingProcess.IsActive = false;
        VotingProcess.TicketId = null;
        return UpdateResult.Ok();
    }

    public UpdateResult AddParticipant(Participant participant)
    {
        if (participant.Role == ParticipantRole.Master)
        {
            return UpdateResult.Error(new() { "This participant can't be added to the game because this game already have a master." });
        }
        if (Participants.Count >= 10)
        {
            return UpdateResult.Error(
                new() { "The maximum number of participants has been exceeded." }
            );
        }

        Participants.Add(participant);
        return UpdateResult.Ok();
    }

    private static bool IsParticipantCanChangeVotingProcess(Participant participant)
    {
        var allowedRolesForChanging = new List<ParticipantRole>() { ParticipantRole.Master, ParticipantRole.Manager };
        return allowedRolesForChanging.Contains(participant.Role);
    }
}
