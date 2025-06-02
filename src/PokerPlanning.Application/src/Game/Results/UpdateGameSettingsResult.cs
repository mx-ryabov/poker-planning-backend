namespace PokerPlanning.Application.src.GameFeature.Results;

public record UpdateGameSettingsResult(
    string Name,
    IEnumerable<GameParticipantResult> UpdatedParticipants,
    bool IsAutoRevealCards,
    int? AutoRevealPeriod
);

