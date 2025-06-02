namespace PokerPlanning.Contracts.src.Game;

public record CreateGameResponse(
    Guid Id,
    string Name,
    string Link,
    CreateGameSettingResponse Settings,
    string MasterToken
);

public record CreateGameSettingResponse(
    bool IsAutoRevealCards,
    int? AutoRevealPeriod
);
