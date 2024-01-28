using PokerPlanning.SDK.sdk.Errors;
using PokerPlanning.SDK.src.Errors;

namespace PokerPlanning.Application.src.GameFeature.Errors;

public class DoVoteException : PlatformException
{
    public DoVoteException(string errors) : base(ExceptionType.BadRequest, $"Impossible to vote because: {errors}")
    {
    }
}
