using FluentAssertions;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.Domain.src.Models.UserAggregate.GuestUserAggregate;
using PokerPlanning.TestUtils.ModelUtils;

namespace PokerPlanning.Domain.UnitTests.Models.GameAggregate;

public class ParticipantTests
{
    [Theory]
    [MemberData(nameof(CreateParticipantData))]
    public void Particiapnt_Create_ReturnsParticipant(ParticipantRole role, string displayName)
    {
        var user = GuestUser.Create(displayName);
        var participant = Participant.Create(displayName, role, user);

        participant.Should().BeOfType<Participant>();
        participant.Role.Should().Be(role);
        participant.DisplayName.Should().Be(displayName);
        participant.User.Should().Be(user);
    }

    public static IEnumerable<object[]> CreateParticipantData =>
        new List<object[]>
        {
            new object[] { ParticipantRole.Master, "Display Name Master" },
            new object[] { ParticipantRole.VotingMember, "Display Name VotingMember" },
            new object[] { ParticipantRole.Spectator, "Display Name Spectator" },
            new object[] { ParticipantRole.Manager, "Display Name Manager" },
        };

    [Theory]
    [MemberData(nameof(DoVoteData))]
    public void Participant_DoVote_ShouldUpdateParticipantOrReturnError(string? voteIdStr, bool isGameActive, bool expectedSuccess, string? expectedVoteIdStr)
    {
        var participant = ParticipantUtils.CreateParticipant(ParticipantRole.VotingMember);
        var game = GameUtils.CreateGame(
            ParticipantUtils.CreateParticipant(ParticipantRole.Master)
        );
        Guid? voteId = voteIdStr != null ? Guid.Parse(voteIdStr) : null;
        Guid? expectedVoteId = expectedVoteIdStr != null ? Guid.Parse(expectedVoteIdStr) : null;
        game.VotingProcess.IsActive = isGameActive;
        participant.Game = game;

        var result = participant.DoVote(voteId);

        result.Success.Should().Be(expectedSuccess);
        participant.VoteId.Should().Be(expectedVoteId);
    }

    public static IEnumerable<object[]> DoVoteData =>
        new List<object[]>
        {
            new object[] { "a9c5f623-88f4-4756-8a84-e3291b503c0d", true, true, "a9c5f623-88f4-4756-8a84-e3291b503c0d" },
            new object[] { null, true, true, null },
            new object[] { "a9c5f623-88f4-4756-8a84-e3291b503c0d", false, false, null },
            new object[] { null, false, false, null },
        };

    [Fact]
    public void Participant_Create_CreatesParticipantOfflineByDefault()
    {
        var participant = ParticipantUtils.CreateParticipant(ParticipantRole.VotingMember);
        participant.Online.Should().BeFalse();
    }

    [Fact]
    public void Participant_GoOnline_MakesUserToBeOnline()
    {
        var participant = ParticipantUtils.CreateParticipant(ParticipantRole.VotingMember);

        participant.GoOnline();
        participant.Online.Should().BeTrue();
    }
    [Fact]
    public void Participant_GoOnffline_MakesOnlineUserToBeOffline()
    {
        var participant = ParticipantUtils.CreateParticipant(ParticipantRole.VotingMember);

        participant.GoOnline();
        participant.Online.Should().BeTrue();
        participant.GoOffline();
        participant.Online.Should().BeFalse();
    }
}