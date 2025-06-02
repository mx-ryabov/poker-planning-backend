using System.Collections.Generic;
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

    public UpdateResultWithData<GameSettingsDTO> UpdateGameSettings(ParticipantRole initiatorRole, UpdateGameSettingsDTO settings)
    {
        if (!IsParticipantCanChangeGameSettings(initiatorRole))
        {
            return UpdateResultWithData<GameSettingsDTO>.Error(
                new() { "This user doesn't have permissions to update game's settings." }
            );
        }

        if (VotingProcess.Status == VotingStatus.InProgress && (settings.IsAutoRevealCards is not null || settings.AutoRevealPeriod is not null))
        {
            return UpdateResultWithData<GameSettingsDTO>.Error(
                new() { "You can't update the auto-revealing settings when the voting process in progress" }
            );
        }

        Name = settings.Name ?? Name;
        UpdateAutoRevealingSetting(settings.IsAutoRevealCards, settings.AutoRevealPeriod);

        if (settings.GameMasterId is not null)
        {
            var result = ChangeGameMaster(settings.GameMasterId.Value);
            if (result.Success && result.Data is not null)
            {
                return UpdateResultWithData<GameSettingsDTO>.Ok(new GameSettingsDTO(
                    Name: Name,
                    IsAutoRevealCards: Settings.IsAutoRevealCards,
                    AutoRevealPeriod: Settings.AutoRevealPeriod,
                    UpdatedParticipants: result.Data
                ));
            }
            else
            {
                return UpdateResultWithData<GameSettingsDTO>.Error(result.Errors);
            }
        }

        return UpdateResultWithData<GameSettingsDTO>.Ok(new GameSettingsDTO(
                    Name: Name,
                    IsAutoRevealCards: Settings.IsAutoRevealCards,
                    AutoRevealPeriod: Settings.AutoRevealPeriod,
                    UpdatedParticipants: new List<Participant>()
                ));
    }

    public UpdateResultWithData<StartVotingProcessDTO> StartVotingProcess(Participant initiator, Guid? ticketId)
    {
        var canStart = IsParticipantCanChangeVotingProcess(initiator);
        if (!canStart)
        {
            return UpdateResultWithData<StartVotingProcessDTO>.Error(
                new() { "This participant isn't allowed to start the voting process." }
            );
        }

        VotingProcess.Status = VotingStatus.InProgress;
        VotingProcess.TicketId = ticketId;
        var now = DateTime.UtcNow;
        VotingProcess.StartTime = now;
        return UpdateResultWithData<StartVotingProcessDTO>.Ok(
                new StartVotingProcessDTO(
                    StartTime: now
                ));
    }

    public UpdateResult RevealCards(Participant initiator)
    {
        var canReveal = IsParticipantCanChangeVotingProcess(initiator);
        if (!canReveal)
        {
            return UpdateResult.Error(
                new() { "This participant isn't allowed to reveal cards in the voting process." }
            );
        }
        if (VotingProcess.Status == VotingStatus.Revealed)
        {
            return UpdateResult.Ok();
        }
        if (VotingProcess.Status != VotingStatus.InProgress)
        {
            return UpdateResult.Error(
                new() { "The Voting Process must be In Progress before the cards are releaved" }
            );
        }

        VotingProcess.Status = VotingStatus.Revealed;
        VotingProcess.StartTime = null;
        return UpdateResult.Ok();
    }

    public UpdateResultWithData<VotingResult> FinishVotingProcess(Participant initiator)
    {
        var canFinish = IsParticipantCanChangeVotingProcess(initiator);
        if (!canFinish)
        {
            return UpdateResultWithData<VotingResult>.Error(
                new() { "This participant isn't allowed to finish the voting process." }
            );
        }

        CollectVotingResults();

        VotingProcess.Status = VotingStatus.Inactive;
        VotingProcess.TicketId = null;
        VotingProcess.StartTime = null;
        Participants.ForEach(p =>
        {
            p.VoteId = null;
            p.Vote = null;
        });
        return UpdateResultWithData<VotingResult>.Ok(VotingResults.Last());
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

    private void UpdateAutoRevealingSetting(bool? IsAutoRevealCards, int? AutoRevealPeriod)
    {
        Settings.IsAutoRevealCards = IsAutoRevealCards ?? Settings.IsAutoRevealCards;
        if (Settings.IsAutoRevealCards)
        {
            Settings.AutoRevealPeriod = AutoRevealPeriod ?? Settings.AutoRevealPeriod;
        }
    }

    private UpdateResultWithData<IEnumerable<Participant>> ChangeGameMaster(Guid newGameMasterId)
    {
        var newMaster = Participants.Find(p => p.Id == newGameMasterId);
        if (newMaster is not null)
        {
            var oldMaster = Participants.Find(p => p.Role == ParticipantRole.Master);
            newMaster.Role = ParticipantRole.Master;
            var participantForUpdate = new List<Participant>() {
                    newMaster
                };
            if (oldMaster is not null)
            {
                oldMaster.Role = ParticipantRole.VotingMember;
                participantForUpdate.Add(oldMaster);
            }
            return UpdateResultWithData<IEnumerable<Participant>>.Ok(participantForUpdate);
        }
        return UpdateResultWithData<IEnumerable<Participant>>
                        .Error(new() {
                            "There is no a participant with the provided id in this game."
                        });
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

    private static bool IsParticipantCanChangeGameSettings(ParticipantRole participantRole)
    {
        return participantRole == ParticipantRole.Master;
    }
}
