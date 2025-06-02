namespace PokerPlanning.Application.src.GameFeature.Results;

public record GameSettingsResult(
    bool IsAutoRevealCards,
    int? AutoRevealPeriod
);
