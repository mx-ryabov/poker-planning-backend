using FluentAssertions;
using Moq;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Commands.DoVote;
using PokerPlanning.Application.src.GameFeature.Commands.FinishVoting;
using PokerPlanning.Application.src.GameFeature.Errors;
using PokerPlanning.Application.src.GameFeature.Results;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.TestUtils.ModelUtils;

namespace PokerPlanning.Application.UnitTests.GameFeature.Commands;

public class FinishVotingCommandHandlerTests
{
    private readonly Mock<IGameRepository> _gameRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly FinishVotingCommandHandler _handler;

    public FinishVotingCommandHandlerTests()
    {
        _gameRepository = new Mock<IGameRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new FinishVotingCommandHandler(
            _gameRepository.Object,
            _unitOfWork.Object
        );
    }

    [Fact]
    public async Task FinishVotingCommandHandler_WhenParticipantFound_AndFinishVotingProcessIsSuccess_ShouldNotThrowExceptions()
    {
        var command = FinishVotingCommandUtils.CreateCommand();
        _gameRepository.Setup(g => g.GetParticipant(
            It.Is<Guid>(gid => gid == command.GameId),
            It.Is<Guid>(uid => uid == command.UserId),
            default
        )).ReturnsAsync(FinishVotingCommandUtils.CreateParticipant(ParticipantRole.Master));
        
        await _handler.Handle(command, default);
        _unitOfWork.Verify(uow => uow.SaveAsync(default), Times.Once);
    }

    [Fact]
    public async Task FinishVotingCommandHandler_WhenParticipantNotFound_ShouldThrowException()
    {
        var command = FinishVotingCommandUtils.CreateCommand();
        _gameRepository.Setup(g => g.GetParticipant(
            It.Is<Guid>(gid => gid == command.GameId),
            It.Is<Guid>(uid => uid == command.UserId),
            default
        )).ReturnsAsync((Participant?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        _unitOfWork.Verify(uow => uow.SaveAsync(default), Times.Never);
    }

    [Fact]
    public async Task FinishVotingCommandHandler_WhenParticipantFound_AndFinishVotingProcessIsFailed_ShouldThrowException()
    {
        var command = FinishVotingCommandUtils.CreateCommand();
        _gameRepository.Setup(g => g.GetParticipant(
            It.Is<Guid>(gid => gid == command.GameId),
            It.Is<Guid>(uid => uid == command.UserId),
            default
        )).ReturnsAsync(FinishVotingCommandUtils.CreateParticipant(ParticipantRole.VotingMember));

        await Assert.ThrowsAsync<ChangingVotingProcessException>(() => _handler.Handle(command, default));
        _unitOfWork.Verify(uow => uow.SaveAsync(default), Times.Never);
    }
}

public class FinishVotingCommandUtils
{
    public static FinishVotingCommand CreateCommand()
    {
        return new FinishVotingCommand(
            GameId: Guid.Parse("80906f84-ace1-404e-a3e7-baf8c06737ac"),
            UserId: Guid.Parse("82e1c6cb-7dcd-40e7-a45b-49b613c1400b")
        );
    }

    public static Participant CreateParticipant(ParticipantRole role)
    {
        var participant = ParticipantUtils.CreateParticipant(role);
        var game = GameUtils.CreateGame(participant);
        game.VotingProcess.IsActive = true;
        participant.Game = game;

        return participant;
    }
}
