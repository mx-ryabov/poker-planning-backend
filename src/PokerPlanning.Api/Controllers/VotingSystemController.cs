using MediatR;
using Microsoft.AspNetCore.Mvc;
using PokerPlanning.Application.src.VotingSystemNS.Queries.VotingSystems;
namespace PokerPlanning.Api.Controllers;

[ApiController]
[Route("api/voting-systems")]
public class VotingSystemController : ControllerBase
{
    private readonly ISender _sender;

    public VotingSystemController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var vsResult = await _sender.Send(new VotingSystemsQuery());
        return Ok(vsResult);
    }
}
