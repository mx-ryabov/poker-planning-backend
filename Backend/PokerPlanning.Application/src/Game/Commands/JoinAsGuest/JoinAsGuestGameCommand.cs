using MediatR;
using PokerPlanning.Application.src.GameFeature.Results;

namespace PokerPlanning.Application.src.GameFeature.Commands.JoinAsGuest;

public record JoinAsGuestGameCommand(
    Guid gameId,
    string DisplayName
) : IRequest<JoinAsGuestGameResult>;
