using FluentAssertions;
using Moq;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Commands.AddTicket;
using PokerPlanning.Application.src.GameFeature.Errors;
using PokerPlanning.Application.src.GameFeature.Results;
using PokerPlanning.Domain.src.Models.GameAggregate;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.TestUtils.ModelUtils;
using PokerPlanning.TestUtils.ModelUtils.Constants;

namespace PokerPlanning.Application.UnitTests.GameFeature.Commands;

public class AddTicketCommandHandlerTests
{
    private readonly AddTicketCommandHandler _handler;
    private readonly Mock<IGameRepository> _gameRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public AddTicketCommandHandlerTests()
    {
        _gameRepository = new Mock<IGameRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new AddTicketCommandHandler(
            _gameRepository.Object,
            _unitOfWork.Object
        );
    }

    [Fact]
    public async Task HandleAddTicketCommand_WhenCommandIsValid_ShouldCreateTicketAndReturnResult()
    {
        var command = AddTicketCommandUtils.CreateCommand();
        _gameRepository.Setup(g => g.GetParticipantRole(
            It.Is<Guid>(gid => gid == command.GameId),
            It.Is<Guid>(gid => gid == command.UserId),
            default
        )).ReturnsAsync(ParticipantRole.Master);
        _gameRepository.Setup(g => g.Get(
            It.Is<Guid>(gid => gid == command.GameId),
            default
        )).ReturnsAsync(AddTicketCommandUtils.CreateGame(2));

        var result = await _handler.Handle(command, default);

        result.Should().BeOfType<TicketResult>();
        
    }

    [Fact]
    public async Task HandleCreateGameCommand_WhenUserNotFound_ShouldThrowNotFoundException()
    {
        var command = AddTicketCommandUtils.CreateCommand();
        _gameRepository.Setup(g => g.GetParticipantRole(
            It.Is<Guid>(gid => gid == command.GameId),
            It.Is<Guid>(gid => gid == command.UserId),
            default
        )).ReturnsAsync((ParticipantRole?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task HandleCreateGameCommand_WhenGameNotFound_ShouldThrowNotFoundException()
    {
        var command = AddTicketCommandUtils.CreateCommand();
        _gameRepository.Setup(g => g.GetParticipantRole(
            It.Is<Guid>(gid => gid == command.GameId),
            It.Is<Guid>(gid => gid == command.UserId),
            default
        )).ReturnsAsync(ParticipantRole.Master);
        _gameRepository.Setup(g => g.Get(
            It.Is<Guid>(gid => gid == command.GameId),
            default
        )).ReturnsAsync((Game?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task HandleCreateGameCommand_WhenParticipantHasNoRightsFound_ShouldThrowAddTicketException()
    {
        var command = AddTicketCommandUtils.CreateCommand();
        _gameRepository.Setup(g => g.GetParticipantRole(
            It.Is<Guid>(gid => gid == command.GameId),
            It.Is<Guid>(gid => gid == command.UserId),
            default
        )).ReturnsAsync(ParticipantRole.VotingMember);
        _gameRepository.Setup(g => g.Get(
            It.Is<Guid>(gid => gid == command.GameId),
            default
        )).ReturnsAsync(AddTicketCommandUtils.CreateGame(2));

        await Assert.ThrowsAsync<AddTicketException>(() => _handler.Handle(command, default));
    }
}

public class AddTicketCommandUtils
{
    public static AddTicketCommand CreateCommand()
    {
        return new AddTicketCommand(
            Title: TicketConstants.Title,
            Type: TicketConstants.Type,
            GameId: Guid.Parse(TicketConstants.GameId),
            UserId: Guid.NewGuid()
        );
    }

    public static Game CreateGame(int participantCount)
    {
        var master = ParticipantUtils.CreateParticipant(ParticipantRole.Master);
        return GameUtils.CreateGame(master, participantCount);
    }
}
