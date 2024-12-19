using MediatR;

namespace PokerPlanning.Application.src.GameFeature.Commands.GoOffline;

public record GoOfflineCommand(
    Guid GameId,
    Guid UserId
) : IRequest;
