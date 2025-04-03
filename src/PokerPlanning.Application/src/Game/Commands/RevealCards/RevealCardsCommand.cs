using MediatR;

namespace PokerPlanning.Application.src.GameFeature.Commands.RevealCards;

public record RevealCardsCommand(
    Guid GameId,
    Guid UserId
) : IRequest;