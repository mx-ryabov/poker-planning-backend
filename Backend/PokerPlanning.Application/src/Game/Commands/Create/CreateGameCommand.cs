using MediatR;
using PokerPlanning.Application.src.Game.Results;

namespace PokerPlanning.Application.src.Game.Commands.Create;

public record CreateGameCommand(
    string Name,
    Guid VotingSystemId,
    CreateGameSettings Settings,
    string CreatorName
) : IRequest<GameResult>;

public record CreateGameSettings(
    bool IsAutoRevealCards
);
