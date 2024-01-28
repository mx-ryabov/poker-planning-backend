using PokerPlanning.SDK.sdk.Errors;
using PokerPlanning.SDK.src.Errors;

namespace PokerPlanning.Application.src.Common.Errors;

public class NotFoundException : PlatformException
{
    public NotFoundException(string entityName = "Object") : base(ExceptionType.ObjectNotFound, $"{entityName} not found.")
    {
    }
}
