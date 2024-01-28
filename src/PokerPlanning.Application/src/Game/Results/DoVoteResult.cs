namespace PokerPlanning.Application.src.GameFeature.Results;

public record DoVoteResult(
    Guid ParticipantId,
    Guid? VoteId
);
