using MediatR;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Queries.GetParticipantById;
using PokerPlanning.Application.src.GameFeature.Results;

namespace PokerPlanning.Application.src.GameFeature.Queries.GetParticipantByGameAndUserId;

public class GetParticipantByGameAndUserIdHandler : IRequestHandler<GetParticipantByGameAndUserIdQuery, GameParticipantResult>
{
    private IGameRepository _gameRepository;
    public GetParticipantByGameAndUserIdHandler(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async Task<GameParticipantResult> Handle(GetParticipantByGameAndUserIdQuery request, CancellationToken cancellationToken)
    {
        var participant = await _gameRepository.GetParticipant(
            gameId: request.GameId,
            userId: request.UserId,
            cancellationToken
        ) ?? throw new NotFoundException("Participant");
        return new GameParticipantResult(
            Id: participant.Id,
            DisplayName: participant.DisplayName,
            Role: participant.Role,
            UserId: participant.UserId
        );
    }
}
