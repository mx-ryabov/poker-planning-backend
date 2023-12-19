namespace PokerPlanning.Contracts.src.Game;

public record GameResponse(
    Guid Id,
    string Name,
    string Link,
    GameSettingResponse Settings,
    string MasterToken
);

public record GameSettingResponse(
    bool IsAutoRevealCards
);
