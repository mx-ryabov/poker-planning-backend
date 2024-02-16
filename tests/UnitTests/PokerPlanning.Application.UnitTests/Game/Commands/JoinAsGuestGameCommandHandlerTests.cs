using FluentAssertions;
using Moq;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Authentication;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Commands.DoVote;
using PokerPlanning.Application.src.GameFeature.Commands.FinishVoting;
using PokerPlanning.Application.src.GameFeature.Commands.JoinAsGuest;
using PokerPlanning.Application.src.GameFeature.Errors;
using PokerPlanning.Application.src.GameFeature.Results;
using PokerPlanning.Domain.src.Models.GameAggregate;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.TestUtils.ModelUtils;

namespace PokerPlanning.Application.UnitTests.GameFeature.Commands;

public class JoinAsGuestGameCommandHandlerTests
{
    private readonly Mock<IGameRepository> _gameRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGenerator;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly JoinAsGuestGameCommandHandler _handler;

    public JoinAsGuestGameCommandHandlerTests()
    {
        _gameRepository = new Mock<IGameRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _jwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        _userRepository = new Mock<IUserRepository>();
        _handler = new JoinAsGuestGameCommandHandler(
            _jwtTokenGenerator.Object,
            _userRepository.Object,
            _gameRepository.Object,
            _unitOfWork.Object
        );
    }

    [Fact]
    public async Task JoinAsGuestGameCommandHandler_WhenGameFound_AndAddParticipantIsSuccess_ShouldReturnJoinAsGuestGameResult()
    {
        var command = JoinAsGuestGameCommandUtils.CreateCommand();
        _gameRepository.Setup(g => g.Get(
            It.Is<Guid>(gid => gid == command.GameId),
            default
        )).ReturnsAsync(JoinAsGuestGameCommandUtils.CreateGame(2));
        _jwtTokenGenerator.Setup(
            jwt => jwt.GenerateToken(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())
        ).Returns(JoinAsGuestGameCommandUtils.Token);

        var result = await _handler.Handle(command, default);

        result.Should().BeOfType<JoinAsGuestGameResult>();
        result.Token.Should().Be(JoinAsGuestGameCommandUtils.Token);
    }

    [Fact]
    public async Task JoinAsGuestGameCommandHandler_WhenGameNotFound_ShouldThrowException()
    {
        var command = JoinAsGuestGameCommandUtils.CreateCommand();
        _gameRepository.Setup(g => g.Get(
            It.Is<Guid>(gid => gid == command.GameId),
            default
        )).ReturnsAsync((Game?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task JoinAsGuestGameCommandHandler_WhenGameFound_AndAddParticipantIsFailed_ShouldThrowException()
    {
        var command = JoinAsGuestGameCommandUtils.CreateCommand();
        _gameRepository.Setup(g => g.Get(
            It.Is<Guid>(gid => gid == command.GameId),
            default
        )).ReturnsAsync(JoinAsGuestGameCommandUtils.CreateGame(10));
        _jwtTokenGenerator.Setup(
            jwt => jwt.GenerateToken(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())
        ).Returns(JoinAsGuestGameCommandUtils.Token);

        await Assert.ThrowsAsync<AddParticipantException>(() => _handler.Handle(command, default));
    }
}

public class JoinAsGuestGameCommandUtils
{
    public static readonly string Token = "token";
    public static JoinAsGuestGameCommand CreateCommand()
    {
        return new JoinAsGuestGameCommand(
            GameId: Guid.Parse("80906f84-ace1-404e-a3e7-baf8c06737ac"),
            DisplayName: "Participant Name"
        );
    }

    public static Game CreateGame(int participantCount)
    {
        var master = ParticipantUtils.CreateParticipant(ParticipantRole.Master);
        return GameUtils.CreateGame(master, participantCount);
    }
}
