using MediatR;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Errors;
using PokerPlanning.Application.src.GameFeature.Results;

namespace PokerPlanning.Application.src.GameFeature.Commands.UpdateTicket;

public class UpdateTicketCommandHandler :
    IRequestHandler<UpdateTicketCommand, TicketResult>
{
    private readonly IGameRepository _gameRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTicketCommandHandler(
        IGameRepository gameRepository,
        IUnitOfWork unitOfWork
    )
    {
        _gameRepository = gameRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<TicketResult> Handle(UpdateTicketCommand command, CancellationToken cancellationToken)
    {
        var currentParticipantRole = await _gameRepository.GetParticipantRole(
            command.GameId,
            command.UserId,
            cancellationToken
        ) ?? throw new NotFoundException("Participant");
        var game = await _gameRepository.Get(command.GameId, cancellationToken) ?? throw new NotFoundException("Game");
        var result = game.UpdateTicket(
            ticketId: command.TicketId,
            data: command.Data,
            currentParticipantRole
        );

        if (!result.Success)
        {
            throw new UpdateTicketException(result.ErrorsString);
        }
        await _unitOfWork.SaveAsync(cancellationToken);

        return new TicketResult(
            Id: result.Data!.Id,
            Title: result.Data!.Title,
            Description: result.Data!.Description,
            Link: result.Data!.Link,
            Type: result.Data!.Type,
            Identifier: result.Data!.Identifier,
            Estimation: result.Data!.Estimation
        );
    }
}
