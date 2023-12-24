namespace PokerPlanning.Application.src.GameFeature.Results;

public record CreateGameResult(
    Guid Id,
    string Name,
    string Link,
    GameSettingsResult Settings,
    List<GameParticipantResult> Participants,
    string MasterToken
);

