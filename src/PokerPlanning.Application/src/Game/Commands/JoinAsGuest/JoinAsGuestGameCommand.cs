using MediatR;
using PokerPlanning.Application.src.GameFeature.Results;

namespace PokerPlanning.Application.src.GameFeature.Commands.JoinAsGuest;

public record JoinAsGuestGameCommand(
    Guid GameId,
    string DisplayName
) : IRequest<JoinAsGuestGameResult>;
