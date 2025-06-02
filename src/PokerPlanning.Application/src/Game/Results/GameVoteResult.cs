namespace PokerPlanning.Application.src.GameFeature.Results;

public record GameVoteResult(
    Guid Id,
    string Value,
    string Suit,
    decimal Order
);
