using System.Threading;
using FluentAssertions;
using Moq;
using PokerPlanning.Application.src.Common.Interfaces.Authentication;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Commands.Create;
using PokerPlanning.Application.src.GameFeature.Results;
using PokerPlanning.Domain.src.Models.GameAggregate;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.Domain.src.Models.UserAggregate.Enums;
using PokerPlanning.Domain.src.Models.UserAggregate.GuestUserAggregate;
using PokerPlanning.TestUtils.ModelUtils.Constants;

namespace PokerPlanning.Application.UnitTests.GameFeature.Commands;

public class CreateGameCommandHandlerTests
{
    private readonly CreateGameCommandHandler _handler;
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGenerator;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IGameRepository> _gameRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public CreateGameCommandHandlerTests()
    {
        _jwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        _userRepository = new Mock<IUserRepository>();
        _gameRepository = new Mock<IGameRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new CreateGameCommandHandler(
            _jwtTokenGenerator.Object,
            _userRepository.Object,
            _gameRepository.Object,
            _unitOfWork.Object
        );
    }

    [Fact]
    public async Task HandleCreateGameCommand_WhenGameCommandIsValid_ShouldCreateAndReturnGame()
    {
        var command = CreateGameCommandUtils.CreateCommand();

        var result = await _handler.Handle(command, default);

        result.Should().BeOfType<CreateGameResult>();
        _userRepository.Verify(
            u => u.CreateGuest(
                It.Is<GuestUser>(gu => gu.DisplayName == command.CreatorName),
                default
            ),
            Times.Once
        );
        _gameRepository.Verify(
            g => g.Create(
                It.Is<Game>(g => g.Name == command.Name
                    && g.VotingSystemId == command.VotingSystemId
                    && g.Settings.IsAutoRevealCards == command.Settings.IsAutoRevealCards
                    && g.Participants.Count == 1
                    && g.Participants[0].Role == ParticipantRole.Master
                ),
                default
            ),
            Times.Once
        );
        _jwtTokenGenerator.Verify(
            uow => uow.GenerateToken(
                command.CreatorName, It.IsAny<Guid>(), UserRole.Guest.ToString()
            ),
            Times.Once
        );
    }

    [Fact]
    public async Task HandleCreateGameCommand_WhenSaveAsyncFailed_ShouldThrowException()
    {
        var command = CreateGameCommandUtils.CreateCommand();
        var cancelationToken = CancellationToken.None;
        _unitOfWork.Setup(uow => uow.SaveAsync(cancelationToken)).ThrowsAsync(new Exception());
        _unitOfWork.Setup(uow => uow.RollbackAsync(cancelationToken)).Verifiable(Times.Once);

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, cancelationToken));

        _unitOfWork.Verify();
    }
}

public class CreateGameCommandUtils
{
    public static CreateGameCommand CreateCommand()
    {
        return new CreateGameCommand(
            Name: GameConstants.Name,
            VotingSystemId: Guid.Parse(GameConstants.VotingSystemId),
            Settings: new CreateGameSettings(
                IsAutoRevealCards: false
            ),
            CreatorName: "Random Name"
        );
    }
}
