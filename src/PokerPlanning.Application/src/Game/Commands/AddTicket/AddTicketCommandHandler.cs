using MediatR;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Errors;
using PokerPlanning.Application.src.GameFeature.Results;
using PokerPlanning.Domain.src.Models.TicketAggregate;

namespace PokerPlanning.Application.src.GameFeature.Commands.AddTicket;

public class AddTicketCommandHandler :
    IRequestHandler<AddTicketCommand, TicketResult>
{
    private readonly IGameRepository _gameRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddTicketCommandHandler(
        IGameRepository gameRepository,
        IUnitOfWork unitOfWork
    )
    {
        _gameRepository = gameRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<TicketResult> Handle(AddTicketCommand command, CancellationToken cancellationToken)
    {
        var ticket = Ticket.CreateNew(
            title: command.Title,
            type: command.Type,
            gameId: command.GameId
        );
        var currentParticipantRole = await _gameRepository.GetParticipantRole(
            command.GameId, 
            command.UserId, 
            cancellationToken
        ) ?? throw new NotFoundException("Participant");
        var game = await _gameRepository.Get(command.GameId, cancellationToken) ?? throw new NotFoundException("Game");

        var result = game.AddTicket(ticket, currentParticipantRole);

        if (!result.Success)
        {
            throw new AddTicketException(result.ErrorsString);
        }

        await _unitOfWork.SaveAsync(cancellationToken);

        return new TicketResult(
            Id: ticket.Id,
            Title: ticket.Title,
            Description: ticket.Description,
            Link: ticket.Link,
            Type: ticket.Type,
            Identifier: ticket.Identifier,
            Estimation: ticket.Estimation
        );
    }
}
