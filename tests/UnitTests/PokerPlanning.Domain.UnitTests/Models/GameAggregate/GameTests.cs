using FluentAssertions;
using PokerPlanning.Domain.src.Common;
using PokerPlanning.Domain.src.Models.GameAggregate;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.TestUtils.ModelUtils;

namespace PokerPlanning.Domain.UnitTests.Models.GameAggregate;

public class GameTests
{
    [Fact]
    public void Game_Create_ReturnsGame()
    {
        var game = GameUtils.CreateGame(
            master: ParticipantUtils.CreateParticipant(ParticipantRole.Master)
        );

        game.Should().BeOfType<Game>();
        game.Participants.Count.Should().Be(1);
    }

    [Theory]
    [MemberData(nameof(StartVotingProcessData))]
    public void Game_StartVotingProcess_ShouldUpdateVotingProcessOrReturnError(ParticipantRole gameChangerRole, string? ticketIdStr, bool expectedSuccess, VotingStatus expectedStatus, string? expectedTicketIdStr)
    {
        Guid? ticketId = ticketIdStr != null ? Guid.Parse(ticketIdStr) : null;
        Guid? expectedTicketId = expectedTicketIdStr != null ? Guid.Parse(expectedTicketIdStr) : null;
        var gameChanger = ParticipantUtils.CreateParticipant(gameChangerRole);
        var game = GameUtils.CreateGame(
            master: ParticipantUtils.CreateParticipant(ParticipantRole.Master)
        );

        var result = game.StartVotingProcess(gameChanger, ticketId);

        result.Success.Should().Be(expectedSuccess);
        game.VotingProcess.Status.Should().Be(expectedStatus);
        game.VotingProcess.TicketId.Should().Be(expectedTicketId);
    }

    public static IEnumerable<object[]> StartVotingProcessData =>
        new List<object[]>
        {
            new object[] { ParticipantRole.Master, null, true, VotingStatus.InProgress, null },
            new object[] { ParticipantRole.Master, "a9c5f623-88f4-4756-8a84-e3291b503c0d", true, VotingStatus.InProgress, "a9c5f623-88f4-4756-8a84-e3291b503c0d" },
            new object[] { ParticipantRole.VotingMember, "a9c5f623-88f4-4756-8a84-e3291b503c0d", false, VotingStatus.Inactive, null },
            new object[] { ParticipantRole.Spectator, "a9c5f623-88f4-4756-8a84-e3291b503c0d", false, VotingStatus.Inactive, null },
            new object[] { ParticipantRole.Manager, "a9c5f623-88f4-4756-8a84-e3291b503c0d", true, VotingStatus.InProgress, "a9c5f623-88f4-4756-8a84-e3291b503c0d" },
        };

    [Theory]
    [MemberData(nameof(RevealCardsVotingProcessData))]
    public void Game_RevealCards_ShouldUpdateVotingProcessOrReturnError(ParticipantRole gameChangerRole, VotingStatus initStatus, bool expectedSuccess, VotingStatus expectedStatus)
    {
        var gameChanger = ParticipantUtils.CreateParticipant(gameChangerRole);
        var game = GameUtils.CreateGame(
            master: ParticipantUtils.CreateParticipant(ParticipantRole.Master)
        );
        game.VotingProcess.Status = initStatus;

        var result = game.RevealCards(gameChanger);

        result.Success.Should().Be(expectedSuccess);
        game.VotingProcess.Status.Should().Be(expectedStatus);
    }

    public static IEnumerable<object[]> RevealCardsVotingProcessData =>
        new List<object[]>
        {
            new object[] { ParticipantRole.Master, VotingStatus.InProgress, true, VotingStatus.Revealed },
            new object[] { ParticipantRole.Master, VotingStatus.Inactive, false, VotingStatus.Inactive },
            new object[] { ParticipantRole.Master, VotingStatus.Revealed, true, VotingStatus.Revealed },
        };

    [Theory]
    [MemberData(nameof(FinishVotingProcessData))]
    public void Game_FinishVotingProcess_ShouldUpdateVotingProcessOrReturnError(
        ParticipantRole gameChangerRole,
        string? ticketIdStr,
        bool expectedSuccess,
        VotingStatus expectedStatus,
        string? expectedTicketIdStr,
        int expectedVotingResultsCount,
        int expectedVotesCount
    )
    {
        Guid? ticketId = ticketIdStr != null ? Guid.Parse(ticketIdStr) : null;
        Guid? expectedTicketId = expectedTicketIdStr != null ? Guid.Parse(expectedTicketIdStr) : null;
        var gameChanger = ParticipantUtils.CreateParticipant(gameChangerRole);
        var game = GameUtils.CreateGame(
            master: ParticipantUtils.CreateParticipant(ParticipantRole.Master)
        );
        game.VotingProcess.Status = VotingStatus.InProgress;
        game.VotingProcess.TicketId = ticketId;

        var result = game.FinishVotingProcess(gameChanger);

        result.Success.Should().Be(expectedSuccess);
        game.VotingProcess.Status.Should().Be(expectedStatus);
        game.VotingProcess.TicketId.Should().Be(expectedTicketId);
        game.VotingResults.Should().HaveCount(expectedVotingResultsCount);
        result.Data?.Votes.Should().HaveCount(expectedVotesCount);
        // TODO: check if FinishVotingProcess returns the latest VotingResult
    }

    public static IEnumerable<object[]> FinishVotingProcessData =>
        new List<object[]>
        {
            new object[] { ParticipantRole.Master, null, true, VotingStatus.Inactive, null, 1, 1 },
            new object[] { ParticipantRole.Master, "a9c5f623-88f4-4756-8a84-e3291b503c0d", true, VotingStatus.Inactive, null, 1, 1 },
            new object[] { ParticipantRole.VotingMember, "a9c5f623-88f4-4756-8a84-e3291b503c0d", false, VotingStatus.InProgress, "a9c5f623-88f4-4756-8a84-e3291b503c0d", 0, 0 },
            new object[] { ParticipantRole.Spectator, "a9c5f623-88f4-4756-8a84-e3291b503c0d", false, VotingStatus.InProgress, "a9c5f623-88f4-4756-8a84-e3291b503c0d", 0, 0 },
            new object[] { ParticipantRole.Manager, "a9c5f623-88f4-4756-8a84-e3291b503c0d", true, VotingStatus.Inactive, null, 1, 1 },
        };

    [Theory]
    [MemberData(nameof(AddParticipantData))]
    public void Game_AddParticipant_ShouldAddOrReturnError(ParticipantRole addingParticipantRole, int initialParticipantsCount, bool expectedSuccess, int expectedParticipantCount)
    {
        var game = GameUtils.CreateGame(
            master: ParticipantUtils.CreateParticipant(ParticipantRole.Master),
            initialParticipantsCount
        );

        var result = game.AddParticipant(ParticipantUtils.CreateParticipant(addingParticipantRole));

        result.Success.Should().Be(expectedSuccess);
        game.Participants.Count.Should().Be(expectedParticipantCount);
    }

    public static IEnumerable<object[]> AddParticipantData =>
        new List<object[]>
        {
            new object[] { ParticipantRole.Master, 0, false, 1 },
            new object[] { ParticipantRole.VotingMember, 0, true, 2 },
            new object[] { ParticipantRole.VotingMember, 9, false, 10 },
            new object[] { ParticipantRole.Spectator, 0, true, 2 },
            new object[] { ParticipantRole.Manager, 0, true, 2 },
        };

    // Add Ticket
    [Theory]
    [MemberData(nameof(AddTicketValidData))]
    public void GameAddTicket_WhenParticipantHasEnoughtRights_ShouldAddTicket(ParticipantRole addingParticipantRole)
    {
        var addingParticipant = ParticipantUtils.CreateParticipant(addingParticipantRole);
        var game = GameUtils.CreateGame(addingParticipant);
        var ticket = TicketUtils.CreateTicket();

        var result = game.AddTicket(ticket, addingParticipantRole);

        result.Success.Should().Be(true);
        game.Tickets.Count.Should().Be(1);
        game.Tickets[0].Should().Be(ticket);
    }

    public static IEnumerable<object[]> AddTicketValidData =>
        new List<object[]>
        {
            new object[] { ParticipantRole.Master },
            new object[] { ParticipantRole.Manager},
        };

    [Theory]
    [MemberData(nameof(AddTicketInvalidData))]
    public void GameAddTicket_WhenParticipantHasNotEnoughtRights_ReturnsError(ParticipantRole addingParticipantRole)
    {
        var addingParticipant = ParticipantUtils.CreateParticipant(addingParticipantRole);
        var game = GameUtils.CreateGame(addingParticipant);
        var ticket = TicketUtils.CreateTicket();

        var result = game.AddTicket(ticket, addingParticipantRole);

        result.Success.Should().Be(false);
        result.Errors.Count.Should().Be(1);
        game.Tickets.Count.Should().Be(0);
    }

    public static IEnumerable<object[]> AddTicketInvalidData =>
        new List<object[]>
        {
            new object[] { ParticipantRole.VotingMember },
            new object[] { ParticipantRole.Spectator},
        };


    // Update Ticket
    [Theory]
    [MemberData(nameof(UpdateTicketValidData))]
    public void GameUpdateTicket_WhenParticipantHasEnoughtRights_ShouldAddTicket(ParticipantRole addingParticipantRole)
    {
        var addingParticipant = ParticipantUtils.CreateParticipant(addingParticipantRole);
        var game = GameUtils.CreateGame(addingParticipant);
        var ticket = TicketUtils.CreateTicket();
        game.AddTicket(ticket, addingParticipantRole);
        var updateData = TicketUtils.GetUpdateTicketDTO();

        var result = game.UpdateTicket(ticket.Id, updateData, addingParticipantRole);

        result.Success.Should().Be(true);
        game.Tickets.Count.Should().Be(1);
        game.Tickets[0].Should().Be(result.Data);
    }

    public static IEnumerable<object[]> UpdateTicketValidData =>
        new List<object[]>
        {
            new object[] { ParticipantRole.Master },
            new object[] { ParticipantRole.Manager},
        };

    [Theory]
    [MemberData(nameof(UpdateTicketInvalidData))]
    public void GameUpdateTicket_WhenModelInvalid_ReturnsError(ParticipantRole addingParticipantRole)
    {
        var addingParticipant = ParticipantUtils.CreateParticipant(addingParticipantRole);
        var game = GameUtils.CreateGame(addingParticipant);
        var ticket = TicketUtils.CreateTicket();
        game.AddTicket(ticket, ParticipantRole.Master);
        var updateData = TicketUtils.GetUpdateTicketDTO();

        var result = game.UpdateTicket(ticket.Id, updateData, addingParticipantRole);

        result.Success.Should().Be(false);
        result.Errors.Count.Should().Be(1);
        game.Tickets.Count.Should().Be(1);
        game.Tickets[0].Should().Be(ticket);
    }

    public static IEnumerable<object[]> UpdateTicketInvalidData =>
        new List<object[]>
        {
            new object[] { ParticipantRole.VotingMember },
            new object[] { ParticipantRole.Spectator},
        };


    // Delete Ticket
    [Theory]
    [MemberData(nameof(DeleteTicketValidData))]
    public void GameDeleteTicket_WhenParticipantHasEnoughtRights_ShouldDeleteTicket(ParticipantRole addingParticipantRole)
    {
        var addingParticipant = ParticipantUtils.CreateParticipant(addingParticipantRole);
        var game = GameUtils.CreateGame(addingParticipant);
        var ticket = TicketUtils.CreateTicket();
        game.AddTicket(ticket, addingParticipantRole);

        var result = game.DeleteTicket(ticket.Id, addingParticipantRole);

        result.Success.Should().Be(true);
        game.Tickets.Count.Should().Be(0);
    }

    public static IEnumerable<object[]> DeleteTicketValidData =>
        new List<object[]>
        {
            new object[] { ParticipantRole.Master },
            new object[] { ParticipantRole.Manager},
        };

    [Theory]
    [MemberData(nameof(DeleteTicketInvalidData))]
    public void GameDeleteTicket_WhenModelInvalid_ReturnsError(ParticipantRole addingParticipantRole)
    {
        var addingParticipant = ParticipantUtils.CreateParticipant(addingParticipantRole);
        var game = GameUtils.CreateGame(addingParticipant);
        var ticket = TicketUtils.CreateTicket();
        game.AddTicket(ticket, ParticipantRole.Master);

        var result = game.DeleteTicket(ticket.Id, addingParticipantRole);

        result.Success.Should().Be(false);
        result.Errors.Count.Should().Be(1);
        game.Tickets.Count.Should().Be(1);
        game.Tickets[0].Should().Be(ticket);
    }

    public static IEnumerable<object[]> DeleteTicketInvalidData =>
        new List<object[]>
        {
            new object[] { ParticipantRole.VotingMember },
            new object[] { ParticipantRole.Spectator},
        };
}
