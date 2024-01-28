using MediatR;
using PokerPlanning.Application.src.GameFeature.Results;

namespace PokerPlanning.Application.src.GameFeature.Queries.GetGame;

public record GetGameQuery(
    Guid GameId
) : IRequest<GetGameResult>;
