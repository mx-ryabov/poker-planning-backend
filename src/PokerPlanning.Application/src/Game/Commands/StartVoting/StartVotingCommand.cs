using MediatR;
using PokerPlanning.Application.src.GameFeature.Results;

namespace PokerPlanning.Application.src.GameFeature.Commands.StartVoting;

public record StartVotingCommand(
    Guid GameId,
    Guid UserId,
    Guid? TicketId,
    Action OnTimeIsUp
) : IRequest<StartVotingResult>;
