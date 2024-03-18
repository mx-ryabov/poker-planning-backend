using PokerPlanning.Domain.src.Models.TicketAggregate.Enums;

namespace PokerPlanning.Contracts.src.Game;

public record AddTicketRequest(
    string Title,
    TicketType Type
);
