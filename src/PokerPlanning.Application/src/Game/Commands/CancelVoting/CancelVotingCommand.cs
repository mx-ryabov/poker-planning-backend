using MediatR;

namespace PokerPlanning.Application.src.GameFeature.Commands.CancelVoting;

public record CancelVotingCommand(
    Guid GameId,
    Guid UserId
) : IRequest;
