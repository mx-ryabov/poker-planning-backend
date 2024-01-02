namespace PokerPlanning.Contracts.src.GameHub;

public record ParticipantVotedResponse(
    Guid ParticipantId,
    Guid? VoteId
);
