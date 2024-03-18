using PokerPlanning.SDK.sdk.Errors;
using PokerPlanning.SDK.src.Errors;

namespace PokerPlanning.Application.src.GameFeature.Errors;

public class DeleteTicketException : PlatformException
{
    public DeleteTicketException(string errors) : base(ExceptionType.BadRequest, $"Impossible to delete the ticket because: {errors}")
    {
    }
}
