namespace PokerPlanning.Application.src.Game.Results;

public record GameResult(
    Guid Id,
    string Name,
    string Link,
    bool IsAutoRevealCards
);
