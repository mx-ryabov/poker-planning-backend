namespace PokerPlanning.Application.src.GameFeature.Results;

public record GameVotingResultVoteResult(
    Guid Id,
    Guid? VoteId,
    GameVoteResult? Vote,
    Guid ParticipantId
);
