using PokerPlanning.Application.src.Common.Interfaces.Services;

namespace PokerPlanning.Infrastructure.src.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}