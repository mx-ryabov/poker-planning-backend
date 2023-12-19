using PokerPlanning.SDK.sdk.Errors;
using PokerPlanning.SDK.src.Errors;

namespace PokerPlanning.Infrastructure.src.Exceptions.ApplicationUser;

public class ApplicationUserCreationException : PlatformException
{
    public ApplicationUserCreationException(string message) : base(ExceptionType.AuthenticationFailed, message)
    {
    }
}
