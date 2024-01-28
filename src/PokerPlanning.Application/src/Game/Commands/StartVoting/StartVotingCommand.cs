using MediatR;

namespace PokerPlanning.Application.src.GameFeature.Commands.StartVoting;

public record StartVotingCommand(
    Guid GameId,
    Guid UserId,
    Guid? TicketId
) : IRequest;
