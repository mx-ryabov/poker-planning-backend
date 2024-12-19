using MediatR;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.GameFeature.Results;
using PokerPlanning.Application.src.Results;
using PokerPlanning.Domain.src.Models.GameAggregate;

namespace PokerPlanning.Application.src.GameFeature.Queries.GetGame;

public class GetGameQueryHandler : IRequestHandler<GetGameQuery, GetGameResult>
{
    private IGameRepository _gameRepository;

    public GetGameQueryHandler(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async Task<GetGameResult> Handle(GetGameQuery request, CancellationToken cancellationToken)
    {
        var game = await _gameRepository.Get(request.GameId, cancellationToken) ?? throw new NotFoundException("Game");
        return MapToResult(game);
    }

    private GetGameResult MapToResult(Game game)
    {
        return new GetGameResult(
            Id: game.Id,
            Name: game.Name,
            Link: game.Link,
            Settings: new GameSettingsResult(
                IsAutoRevealCards: game.Settings.IsAutoRevealCards
            ),
            VotingProcess: new GameVotingProcessResult(
                IsActive: game.VotingProcess.IsActive,
                TicketId: game.VotingProcess.TicketId
            ),
            VotingSystem: new VotingSystemResult(
                Id: game.VotingSystem.Id,
                Name: game.VotingSystem.Name,
                Creator: null,
                Votes: game.VotingSystem.Votes.Select(v => new VotingSystemVoteResult(
                    Id: v.Id,
                    Value: v.Value,
                    Order: v.Order,
                    Suit: v.Suit,
                    VotingSystemId: v.VotingSystemId
                ))
            ),
            Participants: game.Participants.Select(p => new GameParticipantResult(
                Id: p.Id,
                DisplayName: p.DisplayName,
                Online: p.Online,
                Role: p.Role,
                UserId: p.UserId
            )),
            Tickets: game.Tickets.Select(t => new GameTicketResult(
                Id: t.Id,
                Title: t.Title,
                Description: t.Description,
                Link: t.Link,
                Type: t.Type,
                Identifier: t.Identifier,
                Estimation: t.Estimation
            ))
        );
    }
}
