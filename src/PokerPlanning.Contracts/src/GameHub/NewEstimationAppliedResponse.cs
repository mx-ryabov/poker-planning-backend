namespace PokerPlanning.Contracts.src.GameHub;

public record NewEstimationAppliedResponse(
    string TicketIdentifier,
    string Estimation,
    string TicketId
);
