using MediatR;
using PokerPlanning.Application.src.GameFeature.Results;

namespace PokerPlanning.Application.src.GameFeature.Commands.FinishVoting;

public record FinishVotingCommand(
    Guid GameId,
    Guid UserId
) : IRequest<GameVotingResultResult>;
