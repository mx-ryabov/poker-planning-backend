namespace PokerPlanning.Domain.src.Common.DTO;

public record UpdateGameSettingsDTO(
    string? Name = null,
    bool? IsAutoRevealCards = null,
    int? AutoRevealPeriod = null,
    Guid? GameMasterId = null
);
