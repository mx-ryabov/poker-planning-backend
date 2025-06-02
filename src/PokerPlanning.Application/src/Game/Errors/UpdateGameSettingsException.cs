using PokerPlanning.SDK.sdk.Errors;
using PokerPlanning.SDK.src.Errors;

namespace PokerPlanning.Application.src.GameFeature.Errors;

public class UpdateGameSettingsException : PlatformException
{
    public UpdateGameSettingsException(string errors) : base(ExceptionType.BadRequest, $"Impossible to update the game settings because: {errors}")
    {
    }
}
