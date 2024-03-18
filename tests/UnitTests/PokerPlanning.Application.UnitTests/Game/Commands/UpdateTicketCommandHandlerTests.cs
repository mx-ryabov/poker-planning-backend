using FluentAssertions;
using Moq;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Commands.AddTicket;
using PokerPlanning.Application.src.GameFeature.Errors;
using PokerPlanning.Application.src.GameFeature.Results;
using PokerPlanning.Domain.src.Common.DTO;
using PokerPlanning.Domain.src.Models.GameAggregate;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.Domain.src.Models.TicketAggregate;
using PokerPlanning.Domain.src.Models.TicketAggregate.Enums;
using PokerPlanning.TestUtils.ModelUtils;
using PokerPlanning.TestUtils.ModelUtils.Constants;

namespace PokerPlanning.Application.UnitTests.GameFeature.Commands;

public class UpdateTicketCommandHandlerTests
{
    private readonly UpdateTicketCommandHandler _handler;
    private readonly Mock<IGameRepository> _gameRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public UpdateTicketCommandHandlerTests()
    {
        _gameRepository = new Mock<IGameRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new UpdateTicketCommandHandler(
            _gameRepository.Object,
            _unitOfWork.Object
        );
    }

    [Fact]
    public async Task HandleUpdateTicketCommand_WhenCommandIsValid_ShouldUpdateTicketAndReturnResult()
    {
        var initialTicket = Ticket.CreateNew("Ticket Name", TicketType.Task, null);
        var command = UpdateTicketCommandUtils.CreateCommand(initialTicket.Id);
        _gameRepository.Setup(g => g.GetParticipantRole(
            It.Is<Guid>(gid => gid == command.GameId),
            It.Is<Guid>(gid => gid == command.UserId),
            default
        )).ReturnsAsync(ParticipantRole.Master);
        _gameRepository.Setup(g => g.Get(
            It.Is<Guid>(gid => gid == command.GameId),
            default
        )).ReturnsAsync(UpdateTicketCommandUtils.CreateGame(2, initialTicket));

        var result = await _handler.Handle(command, default);

        result.Should().BeOfType<TicketResult>();
        
    }

    [Fact]
    public async Task HandleUpdateTicketCommand_WhenUserNotFound_ShouldThrowNotFoundException()
    {
        var initialTicket = Ticket.CreateNew("Ticket Name", TicketType.Task, null);
        var command = UpdateTicketCommandUtils.CreateCommand(initialTicket.Id);
        _gameRepository.Setup(g => g.GetParticipantRole(
            It.Is<Guid>(gid => gid == command.GameId),
            It.Is<Guid>(gid => gid == command.UserId),
            default
        )).ReturnsAsync((ParticipantRole?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task HandleUpdateTicketCommand_WhenGameNotFound_ShouldThrowNotFoundException()
    {
        var initialTicket = Ticket.CreateNew("Ticket Name", TicketType.Task, null);
        var command = UpdateTicketCommandUtils.CreateCommand(initialTicket.Id);
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
    public async Task HandleUpdateTicketCommand_WhenModelInvalid_ShouldThrowUpdateTicketException()
    {
        var initialTicket = Ticket.CreateNew("Ticket Name", TicketType.Task, null);
        var command = UpdateTicketCommandUtils.CreateCommand(initialTicket.Id);
        _gameRepository.Setup(g => g.GetParticipantRole(
            It.Is<Guid>(gid => gid == command.GameId),
            It.Is<Guid>(gid => gid == command.UserId),
            default
        )).ReturnsAsync(ParticipantRole.VotingMember);
        _gameRepository.Setup(g => g.Get(
            It.Is<Guid>(gid => gid == command.GameId),
            default
        )).ReturnsAsync(UpdateTicketCommandUtils.CreateGame(2, initialTicket));

        await Assert.ThrowsAsync<UpdateTicketException>(() => _handler.Handle(command, default));
    }
}

public class UpdateTicketCommandUtils
{
    public static UpdateTicketCommand CreateCommand(Guid? ticketId)
    {
        return new UpdateTicketCommand(
            TicketId: ticketId ?? Guid.Parse(TicketConstants.TicketId),
            GameId: Guid.Parse(TicketConstants.GameId),
            UserId: Guid.NewGuid(),
            Data: TicketUtils.GetUpdateTicketDTO()
        );
    }

    public static Game CreateGame(int participantCount, Ticket? initialTicket)
    {
        var master = ParticipantUtils.CreateParticipant(ParticipantRole.Master);
        var game = GameUtils.CreateGame(master, participantCount);
        if (initialTicket is not null)
        {
            game.AddTicket(initialTicket, ParticipantRole.Manager);
        }
        return game;
    }
}
