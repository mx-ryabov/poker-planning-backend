using PokerPlanning.SDK.sdk.Errors;
using PokerPlanning.SDK.src.Errors;

namespace PokerPlanning.Application.src.GameFeature.Errors;

public class ChangingVotingProcessException : PlatformException
{
    public ChangingVotingProcessException(string messages) : base(ExceptionType.BadRequest, $"Voting process wasn't changed because: {messages}")
    {
    }
}
