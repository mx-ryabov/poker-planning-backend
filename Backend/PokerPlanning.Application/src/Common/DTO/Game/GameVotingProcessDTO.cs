namespace PokerPlanning.Application.src.Common.DTO.GameFeature;

public record GameVotingProcessDTO(
    Guid? TicketId,
    bool? IsActive
);
