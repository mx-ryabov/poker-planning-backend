using MediatR;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Commands.CancelVoting;
using PokerPlanning.Application.src.GameFeature.Errors;

namespace PokerPlanning.Application.src.GameFeature.Commands.FinishVoting;

public class CancelVotingCommandHandler : IRequestHandler<CancelVotingCommand>
{
    private readonly IGameRepository _gameRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelVotingCommandHandler(IGameRepository gameRepository, IUnitOfWork unitOfWork)
    {
        _gameRepository = gameRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CancelVotingCommand command, CancellationToken cancellationToken)
    {
        var participant = await _gameRepository.GetParticipant(command.GameId, command.UserId, cancellationToken) ?? throw new NotFoundException("Participant");
        var game = await _gameRepository.Get(command.GameId, cancellationToken) ?? throw new NotFoundException("Game");
        var result = game.CancelVotingProcess(participant);

        if (!result.Success)
        {
            throw new ChangingVotingProcessException(String.Join("; ", result.Errors));
        }
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
