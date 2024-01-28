using PokerPlanning.SDK.sdk.Errors;
using PokerPlanning.SDK.src.Errors;

namespace PokerPlanning.Application.src.Authentication.Errors;

public class UserAlreadyExistsException : PlatformException
{
    public UserAlreadyExistsException() : base(ExceptionType.ObjectAlreadyExists, "User with this email already exists.")
    { }
}