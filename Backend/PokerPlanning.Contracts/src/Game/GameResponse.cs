namespace PokerPlanning.Contracts.src.Game;

public record GameResponse(
    Guid Id,
    string Name,
    string Link,
    GameSettingResponse Settings
);

public record GameSettingResponse(
    bool IsAutoRevealCards
);
