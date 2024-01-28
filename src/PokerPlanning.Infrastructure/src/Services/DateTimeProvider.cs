using PokerPlanning.Application.src.Common.Interfaces.Services;

namespace PokerPlanning.Infrastructure.src.Authentication.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}