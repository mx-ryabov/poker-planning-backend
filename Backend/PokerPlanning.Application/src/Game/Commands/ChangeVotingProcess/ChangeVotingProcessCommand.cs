using MediatR;

namespace PokerPlanning.Application.src.GameFeature.Commands.ChangeVotingProcess;

public record ChangeVotingProcessCommand(
    Guid GameId,
    Guid UserId,
    bool IsActive,
    Guid? TicketId
) : IRequest;
