namespace PokerPlanning.Application.src.GameFeature.Results;

public record StartVotingResult(
    DateTime StartTime,
    int? RevealCardsDelay
);
