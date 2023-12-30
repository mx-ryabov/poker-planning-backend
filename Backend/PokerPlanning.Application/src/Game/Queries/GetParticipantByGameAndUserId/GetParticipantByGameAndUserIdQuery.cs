using MediatR;
using PokerPlanning.Application.src.GameFeature.Results;

namespace PokerPlanning.Application.src.GameFeature.Queries.GetParticipantById;

public record GetParticipantByGameAndUserIdQuery(
    Guid GameId,
    Guid UserId
) : IRequest<GameParticipantResult>;
