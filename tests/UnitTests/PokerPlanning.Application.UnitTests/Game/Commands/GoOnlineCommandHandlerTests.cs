using Moq;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Commands.GoOnline;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.TestUtils.ModelUtils;

namespace PokerPlanning.Application.UnitTests.GameFeature.Commands;

public class GoOnlineCommandHandlerTests
{
    private readonly Mock<IGameRepository> _gameRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly GoOnlineCommandHandler _handler;

    public GoOnlineCommandHandlerTests()
    {
        _gameRepository = new Mock<IGameRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new GoOnlineCommandHandler(
            _gameRepository.Object,
            _unitOfWork.Object
        );
    }

    [Fact]
    public async Task GoOnlineCommandHandler_WhenParticipantFound_AndGoOnlineIsSuccess_ShouldNotThrowExceptions()
    {
        var command = GoOnlineCommandUtils.CreateCommand();
        _gameRepository.Setup(g => g.GetParticipant(
            It.Is<Guid>(gid => gid == command.GameId),
            It.Is<Guid>(uid => uid == command.UserId),
            default
        )).ReturnsAsync(GoOnlineCommandUtils.CreateParticipant(ParticipantRole.Master));

        await _handler.Handle(command, default);

        _unitOfWork.Verify(uow => uow.SaveAsync(default), Times.Once);
    }
}

public class GoOnlineCommandUtils
{
    public static GoOnlineCommand CreateCommand()
    {
        return new GoOnlineCommand(
            GameId: Guid.Parse("80906f84-ace1-404e-a3e7-baf8c06737ac"),
            UserId: Guid.Parse("82e1c6cb-7dcd-40e7-a45b-49b613c1400b")
        );
    }

    public static Participant CreateParticipant(ParticipantRole role)
    {
        var masterParticipant = ParticipantUtils.CreateParticipant(role);
        var game = GameUtils.CreateGame(masterParticipant);
        var votingMember = ParticipantUtils.CreateParticipant(ParticipantRole.VotingMember);

        return votingMember;
    }
}