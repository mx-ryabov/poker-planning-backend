namespace PokerPlanning.Application.src.GameFeature.Results;

public record GameVotingResultResult(
    Guid Id,
    Guid? TicketId,
    DateTimeOffset CreatedAt,
    IEnumerable<GameVotingResultVoteResult> Votes
);
