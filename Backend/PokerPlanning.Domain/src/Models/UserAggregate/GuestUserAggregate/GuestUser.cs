using PokerPlanning.Domain.src.Models.UserAggregate.Enums;

namespace PokerPlanning.Domain.src.Models.UserAggregate.GuestUserAggregate;

public class GuestUser : User
{
    public override UserRole Role { get; } = UserRole.Guest;
    public string DisplayName { get; set; } = null!;

    protected GuestUser(Guid id, string displayName) : base(id)
    {
        DisplayName = displayName;
    }

    public static GuestUser Create(string displayName)
    {
        return Create(Guid.NewGuid(), displayName);
    }

    public static GuestUser Create(Guid id, string displayName)
    {
        return new(id, displayName);
    }
}
