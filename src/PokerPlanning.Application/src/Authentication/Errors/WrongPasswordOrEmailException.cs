using PokerPlanning.SDK.sdk.Errors;
using PokerPlanning.SDK.src.Errors;

namespace PokerPlanning.Application.src.Authentication.Errors;

public class WrongPasswordOrEmailException : PlatformException
{
    public WrongPasswordOrEmailException() : base(ExceptionType.AuthenticationFailed, "Wrong password or email.")
    { }
}