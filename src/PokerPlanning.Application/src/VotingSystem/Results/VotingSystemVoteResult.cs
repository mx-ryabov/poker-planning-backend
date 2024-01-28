namespace PokerPlanning.Application.src.Results;

public record VotingSystemVoteResult(
    Guid Id,
    string Value,
    decimal Order,
    string Suit,
    Guid VotingSystemId
);