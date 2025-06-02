using PokerPlanning.Domain.src.Models.GameAggregate.Enums;

namespace PokerPlanning.Application.src.GameFeature.Results;

public record GameVotingProcessResult(
    VotingStatus Status,
    GameTicketResult? Ticket,
    DateTime? StartTime
);
