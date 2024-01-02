using MediatR;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Errors;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;

namespace PokerPlanning.Application.src.GameFeature.Commands.ChangeVotingProcess;

public class ChangeVotingProcessCommandHandler : IRequestHandler<ChangeVotingProcessCommand>
{
    private readonly IGameRepository _gameRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeVotingProcessCommandHandler(IGameRepository gameRepository, IUnitOfWork unitOfWork)
    {
        _gameRepository = gameRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ChangeVotingProcessCommand command, CancellationToken cancellationToken)
    {
        var updatedVotingProcess = new VotingProcess()
        {
            IsActive = command.IsActive,
            TicketId = command.TicketId
        };
        var participant = await _gameRepository.GetParticipant(command.GameId, command.UserId, cancellationToken) ?? throw new NotFoundException("Participant");
        var game = participant.Game;
        var result = game.ChangeVotingProcess(updatedVotingProcess, participant);

        if (!result.Success)
        {
            throw new ChangingVotingProcessException(String.Join("; ", result.Errors));
        }
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
