using MediatR;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.Common.Interfaces.Services;
using PokerPlanning.Application.src.GameFeature.Errors;
using PokerPlanning.Application.src.GameFeature.Results;

namespace PokerPlanning.Application.src.GameFeature.Commands.StartVoting;

public class StartVotingCommandHandler : IRequestHandler<StartVotingCommand, StartVotingResult>
{
    private readonly IGameRepository _gameRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGameTimer _gameTimer;

    public StartVotingCommandHandler(IGameRepository gameRepository, IUnitOfWork unitOfWork, IGameTimer gameTimer)
    {
        _gameRepository = gameRepository;
        _unitOfWork = unitOfWork;
        _gameTimer = gameTimer;
    }

    public async Task<StartVotingResult> Handle(StartVotingCommand command, CancellationToken cancellationToken)
    {
        var participant = await _gameRepository.GetParticipant(command.GameId, command.UserId, cancellationToken) ?? throw new NotFoundException("Participant");
        var game = participant.Game;
        var result = game.StartVotingProcess(participant, command.TicketId);

        if (!result.Success || result.Data is null)
        {
            throw new ChangingVotingProcessException(String.Join("; ", result.Errors));
        }
        await _unitOfWork.SaveAsync(cancellationToken);

        if (game.Settings.IsAutoRevealCards)
        {
            _gameTimer.Start(
                gameId: game.Id,
                userId: command.UserId,
                delay: TimeSpan.FromSeconds(game.Settings.AutoRevealPeriod),
                OnReveal: command.OnTimeIsUp
            );
        }

        return new StartVotingResult(
                StartTime: result.Data.StartTime,
                RevealCardsDelay: game.Settings.IsAutoRevealCards ? game.Settings.AutoRevealPeriod : null
            );
    }
}
