using MediatR;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Errors;
using PokerPlanning.Application.src.GameFeature.Results;

namespace PokerPlanning.Application.src.GameFeature.Commands.FinishVoting;

public class FinishVotingCommandHandler : IRequestHandler<FinishVotingCommand, GameVotingResultResult>
{
    private readonly IGameRepository _gameRepository;
    private readonly IUnitOfWork _unitOfWork;

    public FinishVotingCommandHandler(IGameRepository gameRepository, IUnitOfWork unitOfWork)
    {
        _gameRepository = gameRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GameVotingResultResult> Handle(FinishVotingCommand command, CancellationToken cancellationToken)
    {
        var participant = await _gameRepository.GetParticipant(command.GameId, command.UserId, cancellationToken) ?? throw new NotFoundException("Participant");
        var game = await _gameRepository.Get(command.GameId, cancellationToken) ?? throw new NotFoundException("Game");
        var result = game.FinishVotingProcess(participant);

        if (!result.Success || result.Data is null)
        {
            throw new ChangingVotingProcessException(String.Join("; ", result.Errors));
        }
        await _unitOfWork.SaveAsync(cancellationToken);

        return new GameVotingResultResult(
            Id: result.Data.Id,
            TicketId: result.Data.TicketId,
            CreatedAt: result.Data.CreatedAt,
            Votes: result.Data.Votes.Select(v => new GameVotingResultVoteResult(
                Id: v.Id,
                VoteId: v.VoteId,
                Vote: v.Vote is null ? null : new GameVoteResult(
                    Id: v.Vote.Id,
                    Value: v.Vote.Value,
                    Suit: v.Vote.Suit,
                    Order: v.Vote.Order
                ),
                ParticipantId: v.ParticipantId
            ))
        );
    }
}
