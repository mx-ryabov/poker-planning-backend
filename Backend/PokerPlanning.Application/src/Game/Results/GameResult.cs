using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.Domain.src.Models.UserAggregate;

namespace PokerPlanning.Application.src.Game.Results;

public record GameResult(
    Guid Id,
    string Name,
    string Link,
    GameSettingsResult Settings,
    List<GameParticipantResult> Participants
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

