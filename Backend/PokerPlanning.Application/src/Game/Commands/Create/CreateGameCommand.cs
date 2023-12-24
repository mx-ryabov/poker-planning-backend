using MediatR;
using PokerPlanning.Application.src.GameFeature.Results;

namespace PokerPlanning.Application.src.GameFeature.Commands.Create;

public record CreateGameCommand(
    string Name,
    Guid VotingSystemId,
    CreateGameSettings Settings,
    string CreatorName
) : IRequest<CreateGameResult>;

public record CreateGameSettings(
    bool IsAutoRevealCards
);
