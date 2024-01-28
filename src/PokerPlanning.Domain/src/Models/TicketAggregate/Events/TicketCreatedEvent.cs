using PokerPlanning.Domain.src.BaseModels;

namespace PokerPlanning.Domain.src.Models.TicketAggregate.Events;

public class TicketCreatedEvent : IDomainEvent
{
    public TicketCreatedEvent(Ticket ticket)
    {
        Ticket = ticket;
    }
    public Ticket Ticket { get; set; }
}
