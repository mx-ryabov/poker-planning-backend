using FluentAssertions;
using Moq;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Commands.DoVote;
using PokerPlanning.Application.src.GameFeature.Errors;
using PokerPlanning.Application.src.GameFeature.Results;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.TestUtils.ModelUtils;

namespace PokerPlanning.Application.UnitTests.GameFeature.Commands;

public class DoVoteCommandHandlerTests
{
    private readonly Mock<IGameRepository> _gameRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly DoVoteCommandHandler _handler;

    public DoVoteCommandHandlerTests()
    {
        _gameRepository = new Mock<IGameRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new DoVoteCommandHandler(
            _gameRepository.Object,
            _unitOfWork.Object
        );
    }

    [Fact]
    public async Task DoVoteCommandHandler_WhenParticipantFound_AndGameInProgress_ShouldReturnDoVoteResult()
    {
        var command = DoVoteCommandUtils.CreateCommand(true);
        _gameRepository.Setup(g => g.GetParticipant(
            It.Is<Guid>(gid => gid == command.GameId),
            It.Is<Guid>(uid => uid == command.UserId),
            default
        )).ReturnsAsync(DoVoteCommandUtils.CreateParticipant(VotingStatus.InProgress));

        var result = await _handler.Handle(command, default);

        result.Should().BeOfType<DoVoteResult>();
        result.VoteId.Should().Be(command.VoteId);
        result.ParticipantId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task DoVoteCommandHandler_WhenParticipantNotFound_ShouldThrowNotFoundException()
    {
        var command = DoVoteCommandUtils.CreateCommand(true);
        _gameRepository.Setup(g => g.GetParticipant(
            It.Is<Guid>(gid => gid == command.GameId),
            It.Is<Guid>(uid => uid == command.UserId),
            default
        )).ReturnsAsync((Participant?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task DoVoteCommandHandler_WhenParticipantFound_AndGameIsInactive_ShouldThrowDoVoteException()
    {
        var command = DoVoteCommandUtils.CreateCommand(true);
        _gameRepository.Setup(g => g.GetParticipant(
            It.Is<Guid>(gid => gid == command.GameId),
            It.Is<Guid>(uid => uid == command.UserId),
            default
        )).ReturnsAsync(DoVoteCommandUtils.CreateParticipant(VotingStatus.Inactive));

        await Assert.ThrowsAsync<DoVoteException>(() => _handler.Handle(command, default));
    }
}

public class DoVoteCommandUtils
{
    public static DoVoteCommand CreateCommand(bool withVoteId)
    {
        return new DoVoteCommand(
            GameId: Guid.Parse("80906f84-ace1-404e-a3e7-baf8c06737ac"),
            UserId: Guid.Parse("82e1c6cb-7dcd-40e7-a45b-49b613c1400b"),
            VoteId: withVoteId ? Guid.Parse("f9dc4f96-900a-44d1-b1cb-96292d6f68c3") : null
        );
    }

    public static Participant CreateParticipant(VotingStatus gameVotingStatus)
    {
        var participant = ParticipantUtils.CreateParticipant(ParticipantRole.VotingMember);
        var game = GameUtils.CreateGame(participant);
        game.VotingProcess.Status = gameVotingStatus;
        participant.Game = game;

        return participant;
    }
}
