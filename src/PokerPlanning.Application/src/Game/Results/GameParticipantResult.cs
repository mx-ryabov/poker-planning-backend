using PokerPlanning.Domain.src.Models.GameAggregate.Enums;

namespace PokerPlanning.Application.src.GameFeature.Results;

public record GameParticipantResult(
    Guid Id,
    string DisplayName,
    bool Online,
    ParticipantRole Role,
    Guid? UserId,
    GameVoteResult? Vote
);
