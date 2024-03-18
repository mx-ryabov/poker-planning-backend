using MediatR;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Errors;

namespace PokerPlanning.Application.src.GameFeature.Commands.DeleteTicket;

public class DeleteTicketCommandHandler : IRequestHandler<DeleteTicketCommand, bool>
{
    private readonly IGameRepository _gameRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTicketCommandHandler(
        IGameRepository gameRepository,
        IUnitOfWork unitOfWork
    )
    {
        _gameRepository = gameRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Handle(DeleteTicketCommand command, CancellationToken cancellationToken)
    {
        var currentParticipantRole = await _gameRepository.GetParticipantRole(
            command.GameId,
            command.UserId,
            cancellationToken
        ) ?? throw new NotFoundException("Participant");
        var game = await _gameRepository.Get(command.GameId, cancellationToken) ?? throw new NotFoundException("Game");

        var result = game.DeleteTicket(command.TicketId, currentParticipantRole);

        if (!result.Success)
        {
            throw new DeleteTicketException(result.ErrorsString);
        }
        await _unitOfWork.SaveAsync(cancellationToken);

        return result.Success;
    }
}
