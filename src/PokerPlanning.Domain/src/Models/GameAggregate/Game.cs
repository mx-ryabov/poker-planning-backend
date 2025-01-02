using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Common.DTO;
using PokerPlanning.Domain.src.Common.Results;
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

    public string Name { get; private set; } = null!;
    public string Link { get; private set; } = null!;
    public GameSettings Settings { get; private set; } = null!;
    public VotingProcess VotingProcess { get; private set; } = new();
    public VotingSystem VotingSystem { get; private set; } = null!;
    public Guid VotingSystemId { get; private set; }
    public List<Participant> Participants { get; private set; } = new List<Participant>();
    public List<Ticket> Tickets { get; private set; } = new List<Ticket>();
    public List<VotingResult> VotingResults { get; private set; } = new List<VotingResult>();

    public int TicketsSequenceNumber { get; private set; } = 1;

    public static Game Create(string name, string link, GameSettings settings, Guid votingSystemId, Participant master)
    {
        var game = new Game(Guid.NewGuid())
        {
            Name = name,
            Link = link,
            Settings = settings,
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

        CollectVotingResults();

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

    public UpdateResult AddTicket(Ticket ticket, ParticipantRole addingParticipantRole)
    {
        if (IsParticipantCanAffectTickets(addingParticipantRole))
        {
            return UpdateResult.Error(
                new() { "This user doesn't have enought rights to add tickets to this game." }
            );
        }
        if (ticket.Identifier == null)
        {
            ticket.Update(new(Identifier: GenerateTicketIdentifier()));
            TicketsSequenceNumber++;
        }
        Tickets.Add(ticket);
        return UpdateResult.Ok();
    }

    public UpdateResultWithData<Ticket> UpdateTicket(Guid ticketId, UpdateTicketDTO data, ParticipantRole addingParticipantRole)
    {
        if (IsParticipantCanAffectTickets(addingParticipantRole))
        {
            return UpdateResultWithData<Ticket>.Error(
                new() { "This user doesn't have enought rights to update tickets in this game." }
            );
        }
        var ticket = Tickets.SingleOrDefault(t => t.Id == ticketId);
        if (ticket is null)
        {
            return UpdateResultWithData<Ticket>.Error(
                new() { "Such a ticket does not exist in this game." }
            );
        }
        ticket.Update(data);
        return UpdateResultWithData<Ticket>.Ok(ticket);
    }

    public UpdateResult DeleteTicket(Guid ticketId, ParticipantRole addingParticipantRole)
    {

        if (IsParticipantCanAffectTickets(addingParticipantRole))
        {
            return UpdateResult.Error(
                new() { "This user doesn't have enought rights to delete tickets to this game." }
            );
        }
        var removedCount = Tickets.RemoveAll(t => t.Id == ticketId);
        if (removedCount == 1)
        {
            return UpdateResult.Ok();
        }
        return UpdateResult.Error(new() { "There no tickets in this game with such ID." });
    }

    private void CollectVotingResults()
    {
        var votes = new List<VotingResultVote>();
        foreach (Participant p in Participants)
        {
            votes.Add(VotingResultVote.Create(p.Id, p.VoteId));
            p.DoVote(null);
        }
        var votingResult = VotingResult.Create(Id, votes, VotingProcess.TicketId);
        VotingResults.Add(votingResult);
    }

    private string GenerateTicketIdentifier()
    {
        var prefix = new string(
            Name.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(word => word[0])
                .Take(3)
                .ToArray()
            ).ToUpper();

        return new string(prefix + "-" + TicketsSequenceNumber);
    }

    private static bool IsParticipantCanAffectTickets(ParticipantRole addingParticipantRole)
    {
        return addingParticipantRole != ParticipantRole.Master && addingParticipantRole != ParticipantRole.Manager;
    }

    private static bool IsParticipantCanChangeVotingProcess(Participant participant)
    {
        var allowedRolesForChanging = new List<ParticipantRole>() { ParticipantRole.Master, ParticipantRole.Manager };
        return allowedRolesForChanging.Contains(participant.Role);
    }
}
