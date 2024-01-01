namespace PokerPlanning.Contracts.src.GameHub;

public record VotingProcessChangedResponse(
    bool IsActive,
    Guid? TicketId
);
