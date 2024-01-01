namespace PokerPlanning.Contracts.src.Game;

public record ChangeVotingProcessRequest(
    bool IsActive,
    Guid? TicketId
);
