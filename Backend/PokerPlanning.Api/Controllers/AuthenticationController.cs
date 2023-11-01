using MediatR;
using Microsoft.AspNetCore.Mvc;
using PokerPlanning.Application.src.Authentication.Commands.Register;
using PokerPlanning.Application.src.Authentication.Queries.Login;
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

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);

        AuthenticationResult authResult = await _mediator.Send(command);

        return Ok(MapAuthResult(authResult));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = new LoginQuery(
            request.Email,
            request.Password);

        AuthenticationResult authResult = await _mediator.Send(query);

        return Ok(MapAuthResult(authResult));
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