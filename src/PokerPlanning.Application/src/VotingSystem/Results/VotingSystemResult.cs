namespace PokerPlanning.Application.src.Results;

public record VotingSystemResult(
    Guid Id,
    string Name,
    UserResult? Creator,
    IEnumerable<VotingSystemVoteResult> Votes
);