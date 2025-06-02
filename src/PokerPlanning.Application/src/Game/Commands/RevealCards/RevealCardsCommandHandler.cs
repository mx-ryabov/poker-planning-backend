using MediatR;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.Common.Interfaces.Services;
using PokerPlanning.Application.src.GameFeature.Errors;

namespace PokerPlanning.Application.src.GameFeature.Commands.RevealCards;

public class RevealCardsCommandHandler : IRequestHandler<RevealCardsCommand>
{
    private readonly IGameRepository _gameRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGameTimer _gameTimer;

    public RevealCardsCommandHandler(IGameRepository gameRepository, IUnitOfWork unitOfWork, IGameTimer gameTimer)
    {
        _gameRepository = gameRepository;
        _unitOfWork = unitOfWork;
        _gameTimer = gameTimer;
    }

    public async Task Handle(RevealCardsCommand command, CancellationToken cancellationToken)
    {
        var participant = await _gameRepository.GetParticipant(command.GameId, command.UserId, cancellationToken) ?? throw new NotFoundException("Participant");
        var game = participant.Game;
        var result = game.RevealCards(participant);

        if (!result.Success)
        {
            throw new ChangingVotingProcessException(String.Join("; ", result.Errors));
        }
        _gameTimer.Stop(game.Id);

        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
