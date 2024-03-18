using PokerPlanning.Domain.src.Models.TicketAggregate.Enums;

namespace PokerPlanning.Domain.src.Common.DTO;

// ???
public record UpdateTicketDTO(
    string? Title,
    string? Description,
    string? Link,
    TicketType? Type,
    string? Identifier,
    string? Estimation
);
