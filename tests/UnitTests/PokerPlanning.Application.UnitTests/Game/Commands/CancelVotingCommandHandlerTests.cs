using Moq;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Commands.CancelVoting;
using PokerPlanning.Application.src.GameFeature.Commands.FinishVoting;
using PokerPlanning.Application.src.GameFeature.Errors;
using PokerPlanning.Domain.src.Models.GameAggregate;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.TestUtils.ModelUtils;

namespace PokerPlanning.Application.UnitTests.GameFeature.Commands;

public class CancelVotingCommandHandlerTests
{
    private readonly Mock<IGameRepository> _gameRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly CancelVotingCommandHandler _handler;

    public CancelVotingCommandHandlerTests()
    {
        _gameRepository = new Mock<IGameRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new CancelVotingCommandHandler(
            _gameRepository.Object,
            _unitOfWork.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenGameDoesNotExist()
    {
        // Arrange
        var command = CancelVotingCommandUtils.CreateCommand();
        var participant = CancelVotingCommandUtils.CreateParticipant(ParticipantRole.VotingMember);
        _gameRepository.Setup(g => g.GetParticipant(command.GameId, participant.Id, default))
            .ReturnsAsync(participant);
        _gameRepository.Setup(r => r.Get(command.GameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Game?)null);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        _unitOfWork.Verify(uow => uow.SaveAsync(default), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenUserIsNotMaster()
    {
        var command = CancelVotingCommandUtils.CreateCommand();
        var participant = CancelVotingCommandUtils.CreateParticipant(ParticipantRole.VotingMember);
        var game = participant.Game!;
        _gameRepository.Setup(g => g.GetParticipant(command.GameId, command.UserId, default))
            .ReturnsAsync(participant);
        _gameRepository.Setup(g => g.Get(command.GameId, default))
            .ReturnsAsync(game);

        await Assert.ThrowsAsync<ChangingVotingProcessException>(() => _handler.Handle(command, default));
        _unitOfWork.Verify(uow => uow.SaveAsync(default), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldCancelVoting_WhenAllConditionsAreMet()
    {
        // Arrange
        var command = CancelVotingCommandUtils.CreateCommand();
        var participant = CancelVotingCommandUtils.CreateParticipant(ParticipantRole.Master);
        var game = participant.Game!;
        _gameRepository.Setup(g => g.GetParticipant(command.GameId, command.UserId, default))
            .ReturnsAsync(participant);
        _gameRepository.Setup(r => r.Get(command.GameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWork.Verify(uow => uow.SaveAsync(default), Times.Once);
    }
}

public class CancelVotingCommandUtils
{
    public static CancelVotingCommand CreateCommand()
    {
        return new CancelVotingCommand(
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

    public static Game CreateGame()
    {
        var master = ParticipantUtils.CreateParticipant(ParticipantRole.Master);
        return GameUtils.CreateGame(master, 1);
    }
}
