using Moq;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.Common.Interfaces.Services;
using PokerPlanning.Application.src.GameFeature.Commands.RevealCards;
using PokerPlanning.Application.src.GameFeature.Errors;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.TestUtils.ModelUtils;

namespace PokerPlanning.Application.UnitTests.GameFeature.Commands;

public class RevealCardsCommandHandlerTests
{
    private readonly Mock<IGameRepository> _gameRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IGameTimer> _gameTimer;
    private readonly RevealCardsCommandHandler _handler;

    public RevealCardsCommandHandlerTests()
    {
        _gameRepository = new Mock<IGameRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _gameTimer = new Mock<IGameTimer>();
        _handler = new RevealCardsCommandHandler(
            _gameRepository.Object,
            _unitOfWork.Object,
            _gameTimer.Object
        );
    }

    [Fact]
    public async Task RevealCardsCommandHandler_WhenParticipantFound_AndRevealingCardsIsSuccess_ShouldNotThrowExceptions()
    {
        var command = RevealCardsCommandUtils.CreateCommand();
        _gameRepository.Setup(g => g.GetParticipant(
            It.Is<Guid>(gid => gid == command.GameId),
            It.Is<Guid>(uid => uid == command.UserId),
            default
        )).ReturnsAsync(RevealCardsCommandUtils.CreateParticipant(ParticipantRole.Master));

        await _handler.Handle(command, default);
        _unitOfWork.Verify(uow => uow.SaveAsync(default), Times.Once);
    }

    [Fact]
    public async Task RevealCardsCommandHandler_WhenParticipantNotFound_ShouldThrowException()
    {
        var command = RevealCardsCommandUtils.CreateCommand();
        _gameRepository.Setup(g => g.GetParticipant(
            It.Is<Guid>(gid => gid == command.GameId),
            It.Is<Guid>(uid => uid == command.UserId),
            default
        )).ReturnsAsync((Participant?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        _unitOfWork.Verify(uow => uow.SaveAsync(default), Times.Never);
    }

    [Fact]
    public async Task RevealCardsCommandHandler_WhenParticipantFound_AndRevealCardsIsFailed_ShouldThrowException()
    {
        var command = RevealCardsCommandUtils.CreateCommand();
        _gameRepository.Setup(g => g.GetParticipant(
            It.Is<Guid>(gid => gid == command.GameId),
            It.Is<Guid>(uid => uid == command.UserId),
            default
        )).ReturnsAsync(RevealCardsCommandUtils.CreateParticipant(ParticipantRole.VotingMember));

        await Assert.ThrowsAsync<ChangingVotingProcessException>(() => _handler.Handle(command, default));
        _unitOfWork.Verify(uow => uow.SaveAsync(default), Times.Never);
    }
}



public class RevealCardsCommandUtils
{
    public static RevealCardsCommand CreateCommand()
    {
        return new RevealCardsCommand(
            GameId: Guid.Parse("80906f84-ace1-404e-a3e7-baf8c06737ac"),
            UserId: Guid.Parse("82e1c6cb-7dcd-40e7-a45b-49b613c1400b")
        );
    }

    public static Participant CreateParticipant(ParticipantRole role)
    {
        var participant = ParticipantUtils.CreateParticipant(role);
        var game = GameUtils.CreateGame(participant);
        game.VotingProcess.Status = VotingStatus.InProgress;
        participant.Game = game;

        return participant;
    }
}
