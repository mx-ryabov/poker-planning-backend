using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PokerPlanning.Application.src.GameFeature.Queries.GetParticipantById;
using PokerPlanning.Contracts.src.GameHub;

namespace PokerPlanning.Api.Hubs;

[Authorize]
public class GameHub : Hub
{
    private readonly ISender _sender;

    public GameHub(ISender sender)
    {
        _sender = sender;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var gameId = Convert.ToString(httpContext?.Request.Query["gameId"]);
        var userId = Context.UserIdentifier;
        if (gameId == null || userId == null)
        {
            Context.Abort();
            return;
        }
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        var participantResult = await _sender.Send(new GetParticipantByGameAndUserIdQuery(
            GameId: new Guid(gameId),
            UserId: new Guid(userId)
        ));
        await Clients.Group(gameId).SendAsync("ParticipantJoined", participantResult);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var httpContext = Context.GetHttpContext();
        var gameId = Convert.ToString(httpContext?.Request.Query["gameId"]);
        var userId = Context.UserIdentifier;
        if (gameId != null && userId != null)
        {
            await Clients.Group(gameId).SendAsync("ParticipantLeft", new ParticipantLeftResponse(UserId: userId));
        }
        await base.OnDisconnectedAsync(exception);
    }
}
