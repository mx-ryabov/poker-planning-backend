using MediatR;
using Microsoft.AspNetCore.Mvc;
using PokerPlanning.Application.src.EmailToNotifyFeature.Commands;
using PokerPlanning.Contracts.src.EmailToNotify;
namespace PokerPlanning.Api.Controllers;

[ApiController]
[Route("api/emails-to-notify")]
public class EmailCollectionController : ControllerBase
{
    private readonly ISender _sender;

    public EmailCollectionController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<ActionResult> Collect(CollectEmailToNotifyRequest request)
    {
        await _sender.Send(new CollectEmailToNotiyCommand(request.Email));
        return Ok();
    }
}
