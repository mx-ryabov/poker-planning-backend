namespace PokerPlanning.Application.src.GameFeature.Results;

public record GameVotingProcessResult(
    bool IsActive,
    Guid? TicketId
);
