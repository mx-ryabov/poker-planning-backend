using MediatR;
using PokerPlanning.Application.src.GameFeature.Results;
using PokerPlanning.Domain.src.Common.DTO;

namespace PokerPlanning.Application.src.GameFeature.Commands.UpdateTicket;

public record UpdateTicketCommand(
    Guid TicketId,
    Guid GameId,
    Guid UserId,
    UpdateTicketDTO Data
) : IRequest<TicketResult>;
