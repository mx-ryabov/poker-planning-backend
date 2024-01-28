namespace PokerPlanning.Application.src.Common.DTO.GameFeature;

public record GameDTO(
    Guid Id,
    string? Name,
    GameSettingsDTO? Settings,
    GameVotingProcessDTO? VotingProcess
);
