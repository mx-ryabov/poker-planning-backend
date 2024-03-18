using PokerPlanning.SDK.sdk.Errors;
using PokerPlanning.SDK.src.Errors;

namespace PokerPlanning.Application.src.GameFeature.Errors;

public class UpdateTicketException : PlatformException
{
    public UpdateTicketException(string errors) : base(ExceptionType.BadRequest, $"Impossible to update the ticket because: {errors}")
    {
    }
}
