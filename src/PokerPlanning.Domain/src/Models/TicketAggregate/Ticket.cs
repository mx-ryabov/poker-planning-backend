using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Models.GameAggregate;
using PokerPlanning.Domain.src.Models.TicketAggregate.Enums;
using PokerPlanning.Domain.src.Models.TicketAggregate.Events;

namespace PokerPlanning.Domain.src.Models.TicketAggregate;

public class Ticket : AggregateRoot<Guid>
{
    protected Ticket(Guid id) : base(id)
    {
    }

    public required string Title { get; set; }
    public string Description { get; set; } = "";
    public string? Link { get; set; } = null;
    public TicketType? Type { get; set; } = null;
    public string? Identifier { get; set; } = null;
    public string? Estimation { get; set; } = null;
    public required Game Game { get; set; }

    public static Ticket CreateNew(string title, TicketType type, Game game)
    {
        var ticket = new Ticket(Guid.NewGuid())
        {
            Title = title,
            Type = type,
            Game = game
        };
        ticket.AddDomainEvent(new TicketCreatedEvent(ticket));
        return ticket;
    }
}
