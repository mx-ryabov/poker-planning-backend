using MediatR;

namespace PokerPlanning.Application.src.GameFeature.Commands.FinishVoting;

public record FinishVotingCommand(
    Guid GameId,
    Guid UserId
) : IRequest;
