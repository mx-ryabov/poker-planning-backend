namespace PokerPlanning.Contracts.src.Game;

public record UpdateGameSettingsRequest(
    string? Name = null,
    bool? IsAutoRevealCards = null,
    int? AutoRevealPeriod = null,
    Guid? GameMasterId = null
);
