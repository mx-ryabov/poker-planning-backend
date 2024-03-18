using MediatR;

namespace PokerPlanning.Application.src.GameFeature.Commands.DeleteTicket;

public record DeleteTicketCommand(Guid TicketId, Guid GameId, Guid UserId) : IRequest<bool>;
