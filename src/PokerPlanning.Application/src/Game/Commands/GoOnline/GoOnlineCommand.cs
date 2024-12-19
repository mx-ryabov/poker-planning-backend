using MediatR;

namespace PokerPlanning.Application.src.GameFeature.Commands.GoOnline;

public record GoOnlineCommand(
    Guid GameId,
    Guid UserId
) : IRequest;
