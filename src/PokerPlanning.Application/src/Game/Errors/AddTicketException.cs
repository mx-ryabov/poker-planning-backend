using PokerPlanning.SDK.sdk.Errors;
using PokerPlanning.SDK.src.Errors;

namespace PokerPlanning.Application.src.GameFeature.Errors;

public class AddTicketException : PlatformException
{
    public AddTicketException(string errors) : base(ExceptionType.BadRequest, $"Impossible to add the ticket because: {errors}")
    {
    }
}
