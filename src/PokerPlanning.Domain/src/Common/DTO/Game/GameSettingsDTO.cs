using PokerPlanning.Domain.src.Models.GameAggregate.Entities;

namespace PokerPlanning.Domain.src.Common.DTO;

public record GameSettingsDTO(
    string Name,
    bool IsAutoRevealCards,
    int? AutoRevealPeriod,
    IEnumerable<Participant> UpdatedParticipants
);
