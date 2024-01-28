using PokerPlanning.SDK.src.Errors;

namespace PokerPlanning.SDK.sdk.Errors;

public class PlatformException : Exception
{
    public ExceptionType Type { get; private set; }

    public PlatformException(
        ExceptionType type,
        string message) : base(message)
    {
        Type = type;
    }

    public PlatformException(
        ExceptionType type,
        string message,
        Exception? innerException) : base(message, innerException)
    {
        Type = type;
    }
}