using PokerPlanning.Domain.src.Models.GameAggregate.Enums;

namespace PokerPlanning.Application.src.GameFeature.Results;

public record GameResult(
    Guid Id,
    string Name,
    string Link,
    GameSettingsResult Settings,
    List<GameParticipantResult> Participants,
    string MasterToken
);

public record GameSettingsResult(
    bool IsAutoRevealCards
);

public record GameParticipantResult(
    Guid Id,
    string DisplayName,
    ParticipantRole Role,
    Guid? UserId
);

