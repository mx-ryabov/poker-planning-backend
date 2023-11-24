using MediatR;
using PokerPlanning.Domain.src.Models.TicketAggregate.Events;

namespace PokerPlanning.Application.src.Game.EventHandlers;

public class NotifyParticipantsOnTicketCreated : INotificationHandler<TicketCreatedEvent>
{
    public Task Handle(TicketCreatedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
