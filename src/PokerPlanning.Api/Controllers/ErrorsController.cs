using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PokerPlanning.SDK.sdk.Errors;
using PokerPlanning.SDK.src.Errors;

namespace PokerPlanning.Api.Controllers;

public class ErrorsController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (exception is PlatformException platformException)
        {
            return Problem(
                detail: platformException.Message,
                statusCode: (int)GetHttpStatusCode(platformException.Type));
        }
        return Problem();
    }

    private HttpStatusCode GetHttpStatusCode(ExceptionType exceptionType)
    {
        return exceptionType switch
        {
            ExceptionType.AuthenticationFailed => HttpStatusCode.Unauthorized,
            ExceptionType.AccessDenied => HttpStatusCode.Forbidden,
            ExceptionType.BadRequest => HttpStatusCode.BadRequest,
            ExceptionType.ObjectAlreadyExists => HttpStatusCode.Conflict,
            ExceptionType.ObjectNotFound => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError
        };
    }
}