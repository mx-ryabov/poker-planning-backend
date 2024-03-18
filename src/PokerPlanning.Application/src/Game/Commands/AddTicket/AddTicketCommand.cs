using MediatR;
using PokerPlanning.Application.src.GameFeature.Results;
using PokerPlanning.Domain.src.Models.TicketAggregate.Enums;

namespace PokerPlanning.Application.src.GameFeature.Commands.AddTicket;

public record AddTicketCommand(
    string Title,
    TicketType Type,
    Guid GameId,
    Guid UserId
) : IRequest<TicketResult>;
