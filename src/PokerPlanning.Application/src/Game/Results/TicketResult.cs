using PokerPlanning.Domain.src.Models.TicketAggregate.Enums;

namespace PokerPlanning.Application.src.GameFeature.Results;

public record TicketResult(
    Guid Id,
    string Title,
    string Description,
    string? Link,
    TicketType? Type,
    string Identifier,
    string? Estimation
);
