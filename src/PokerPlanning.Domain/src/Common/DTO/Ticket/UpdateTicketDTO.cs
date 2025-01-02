using PokerPlanning.Domain.src.Models.TicketAggregate.Enums;

namespace PokerPlanning.Domain.src.Common.DTO;

// ???
public record UpdateTicketDTO(
    string? Title = null,
    string? Description = null,
    string? Link = null,
    TicketType? Type = null,
    string? Identifier = null,
    string? Estimation = null
);
