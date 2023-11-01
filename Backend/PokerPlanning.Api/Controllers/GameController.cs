using MediatR;
using Microsoft.AspNetCore.Mvc;
using PokerPlanning.Application.src.Game.Commands.Create;
using PokerPlanning.Application.src.Game.Results;
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

    [HttpPost("")]
    public async Task<IActionResult> Create(GameRequest req)
    {
        var command = new CreateGameCommand(
            Name: req.Name,
            VotingSystemId: req.VotingSystemId,
            IsAutoRevealCards: req.IsAutoRevealCards
        );

        GameResult gameResult = await _sender.Send(command);

        var response = new GameResponse(
            Id: gameResult.Id,
            Name: gameResult.Name,
            Link: gameResult.Link,
            Settings: new GameSettingResponse(
                IsAutoRevealCards: gameResult.IsAutoRevealCards
            )
        );
        return Ok(response);
    }
}
