using MediatR;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Errors;
using PokerPlanning.Application.src.GameFeature.Results;

namespace PokerPlanning.Application.src.GameFeature.Commands.UpdateGameSettings;

public class UpdateGameSettingsCommandHandler :
    IRequestHandler<UpdateGameSettingsCommand, UpdateGameSettingsResult>
{
    private readonly IGameRepository _gameRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGameSettingsCommandHandler(
        IGameRepository gameRepository,
        IUnitOfWork unitOfWork
    )
    {
        _gameRepository = gameRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<UpdateGameSettingsResult> Handle(UpdateGameSettingsCommand command, CancellationToken cancellationToken)
    {
        var currentParticipantRole = await _gameRepository.GetParticipantRole(
            command.GameId,
            command.UserId,
            cancellationToken
        ) ?? throw new NotFoundException("Participant");
        var game = await _gameRepository.Get(command.GameId, cancellationToken) ?? throw new NotFoundException("Game");

        var result = game.UpdateGameSettings(currentParticipantRole, command.Data);

        if (!result.Success || result.Data is null)
        {
            throw new UpdateGameSettingsException(result.ErrorsString);
        }
        await _unitOfWork.SaveAsync(cancellationToken);

        return new UpdateGameSettingsResult(
            Name: result.Data.Name,
            UpdatedParticipants: result.Data.UpdatedParticipants.Select(p => new GameParticipantResult(
                Id: p.Id,
                DisplayName: p.DisplayName,
                Online: p.Online,
                Role: p.Role,
                UserId: p.UserId,
                Vote: p.Vote is null ? null : new GameVoteResult(
                    Id: p.Vote.Id,
                    Value: p.Vote.Value,
                    Suit: p.Vote.Suit,
                    Order: p.Vote.Order
                )
            )),
            IsAutoRevealCards: result.Data.IsAutoRevealCards,
            AutoRevealPeriod: result.Data.AutoRevealPeriod
        );
    }
}
