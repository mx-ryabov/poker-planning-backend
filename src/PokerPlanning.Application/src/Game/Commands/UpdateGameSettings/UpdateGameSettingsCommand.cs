using MediatR;
using PokerPlanning.Application.src.GameFeature.Results;
using PokerPlanning.Domain.src.Common.DTO;

namespace PokerPlanning.Application.src.GameFeature.Commands.UpdateGameSettings;

public record UpdateGameSettingsCommand(
    Guid GameId,
    Guid UserId,
    UpdateGameSettingsDTO Data
) : IRequest<UpdateGameSettingsResult>;
