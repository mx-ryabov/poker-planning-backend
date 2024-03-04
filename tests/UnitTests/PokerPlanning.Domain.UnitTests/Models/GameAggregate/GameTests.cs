using FluentAssertions;
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
    public void Game_StartVotingProcess_ShouldUpdateVotingProcessOrReturnError(ParticipantRole gameChangerRole, string? ticketIdStr, bool expectedSuccess, bool expectedIsActive, string? expectedTicketIdStr)
    {
        Guid? ticketId = ticketIdStr != null ? Guid.Parse(ticketIdStr) : null;
        Guid? expectedTicketId = expectedTicketIdStr != null ? Guid.Parse(expectedTicketIdStr) : null;
        var gameChanger = ParticipantUtils.CreateParticipant(gameChangerRole);
        var game = GameUtils.CreateGame(
            master: ParticipantUtils.CreateParticipant(ParticipantRole.Master)
        );

        var result = game.StartVotingProcess(gameChanger, ticketId);

        result.Success.Should().Be(expectedSuccess);
        game.VotingProcess.IsActive.Should().Be(expectedIsActive);
        game.VotingProcess.TicketId.Should().Be(expectedTicketId);
    }

    public static IEnumerable<object[]> StartVotingProcessData =>
        new List<object[]>
        {
            new object[] { ParticipantRole.Master, null, true, true, null },
            new object[] { ParticipantRole.Master, "a9c5f623-88f4-4756-8a84-e3291b503c0d", true, true, "a9c5f623-88f4-4756-8a84-e3291b503c0d" },
            new object[] { ParticipantRole.VotingMember, "a9c5f623-88f4-4756-8a84-e3291b503c0d", false, false, null },
            new object[] { ParticipantRole.Spectator, "a9c5f623-88f4-4756-8a84-e3291b503c0d", false, false, null },
            new object[] { ParticipantRole.Manager, "a9c5f623-88f4-4756-8a84-e3291b503c0d", true, true, "a9c5f623-88f4-4756-8a84-e3291b503c0d" },
        };

    [Theory]
    [MemberData(nameof(FinishVotingProcessData))]
    public void Game_FinishVotingProcess_ShouldUpdateVotingProcessOrReturnError(ParticipantRole gameChangerRole, string? ticketIdStr, bool expectedSuccess, bool expectedIsActive, string? expectedTicketIdStr, int expectedVotingResultsCount)
    {
        Guid? ticketId = ticketIdStr != null ? Guid.Parse(ticketIdStr) : null;
        Guid? expectedTicketId = expectedTicketIdStr != null ? Guid.Parse(expectedTicketIdStr) : null;
        var gameChanger = ParticipantUtils.CreateParticipant(gameChangerRole);
        var game = GameUtils.CreateGame(
            master: ParticipantUtils.CreateParticipant(ParticipantRole.Master)
        );
        game.VotingProcess.IsActive = true;
        game.VotingProcess.TicketId = ticketId;

        var result = game.FinishVotingProcess(gameChanger);

        result.Success.Should().Be(expectedSuccess);
        game.VotingProcess.IsActive.Should().Be(expectedIsActive);
        game.VotingProcess.TicketId.Should().Be(expectedTicketId);
        game.VotingResults.Should().HaveCount(expectedVotingResultsCount);
    }

    public static IEnumerable<object[]> FinishVotingProcessData =>
        new List<object[]>
        {
            new object[] { ParticipantRole.Master, null, true, false, null, 1 },
            new object[] { ParticipantRole.Master, "a9c5f623-88f4-4756-8a84-e3291b503c0d", true, false, null, 1 },
            new object[] { ParticipantRole.VotingMember, "a9c5f623-88f4-4756-8a84-e3291b503c0d", false, true, "a9c5f623-88f4-4756-8a84-e3291b503c0d", 0 },
            new object[] { ParticipantRole.Spectator, "a9c5f623-88f4-4756-8a84-e3291b503c0d", false, true, "a9c5f623-88f4-4756-8a84-e3291b503c0d", 0 },
            new object[] { ParticipantRole.Manager, "a9c5f623-88f4-4756-8a84-e3291b503c0d", true, false, null, 1 },
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
}
