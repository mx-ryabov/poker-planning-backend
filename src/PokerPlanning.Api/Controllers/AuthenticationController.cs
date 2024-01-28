using MediatR;
using Microsoft.AspNetCore.Mvc;
using PokerPlanning.Application.src.Services.Authentication;
using PokerPlanning.Contracts.src.Authentication;

namespace PokerPlanning.Api.Controllers;


[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ISender _mediator;

    public AuthenticationController(ISender mediator)
    {
        _mediator = mediator;
    }

    private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
            authResult.Id,
            authResult.FirstName,
            authResult.LastName,
            authResult.Email,
            authResult.Token
        );
    }
}