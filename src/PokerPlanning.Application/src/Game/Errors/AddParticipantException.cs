using PokerPlanning.SDK.sdk.Errors;
using PokerPlanning.SDK.src.Errors;

namespace PokerPlanning.Application.src.GameFeature.Errors;

public class AddParticipantException : PlatformException
{
    public AddParticipantException(string errors) : base(ExceptionType.BadRequest, $"Impossible to add the new participant because: {errors}")
    {
    }
}
