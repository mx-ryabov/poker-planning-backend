using MediatR;
using PokerPlanning.Application.src.Game.Results;

namespace PokerPlanning.Application.src.Game.Commands.Create;

public record CreateGameCommand(
    string Name,
    Guid VotingSystemId,
    bool? IsAutoRevealCards
) : IRequest<GameResult>;


