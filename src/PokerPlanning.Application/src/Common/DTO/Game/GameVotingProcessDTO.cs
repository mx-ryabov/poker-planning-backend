using PokerPlanning.Domain.src.Models.GameAggregate.Enums;

namespace PokerPlanning.Application.src.Common.DTO.GameFeature;

public record GameVotingProcessDTO(
    Guid? TicketId,
    VotingStatus? Status
);
