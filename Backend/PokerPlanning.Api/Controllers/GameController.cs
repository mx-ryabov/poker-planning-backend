using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokerPlanning.Application.src.GameFeature.Commands.Create;
using PokerPlanning.Application.src.GameFeature.Commands.JoinAsGuest;
using PokerPlanning.Application.src.GameFeature.Queries.GetGame;
using PokerPlanning.Application.src.GameFeature.Results;
using PokerPlanning.Contracts.src.Game;

namespace PokerPlanning.Api.Controllers;

[ApiController]
[Route("api/games")]
public class GameController : ControllerBase
{
    private readonly ISender _sender;

    public GameController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("getById")]
    [Authorize]
    public async Task<ActionResult> Get([FromQuery] Guid gameId)
    {
        GetGameResult gameResult = await _sender.Send(new GetGameQuery(gameId));
        return Ok(gameResult);
    }

    [HttpPost("")]
    public async Task<ActionResult> Create([FromBody] CreateGameRequest req)
    {
        var command = new CreateGameCommand(
            Name: req.Name,
            VotingSystemId: req.VotingSystemId,
            Settings: new CreateGameSettings(
                IsAutoRevealCards: req.IsAutoRevealCards
            ),
            CreatorName: req.CreatorName
        );

        CreateGameResult gameResult = await _sender.Send(command);

        var response = new CreateGameResponse(
            Id: gameResult.Id,
            Name: gameResult.Name,
            Link: gameResult.Link,
            Settings: new CreateGameSettingResponse(
                IsAutoRevealCards: gameResult.Settings.IsAutoRevealCards
            ),
            MasterToken: gameResult.MasterToken
        );
        return Ok(response);
    }

    [HttpPost("joinAsGuest")]
    public async Task<ActionResult> JoinAsGuest([FromBody] JoinAsGuestRequest req)
    {
        JoinAsGuestGameResult joinAsGuestResult = await _sender.Send(new JoinAsGuestGameCommand(req.GameId, req.DisplayName));
        return Ok(joinAsGuestResult);
    }
}
