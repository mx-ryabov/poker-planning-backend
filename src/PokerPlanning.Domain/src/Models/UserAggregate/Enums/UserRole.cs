using System.ComponentModel;

namespace PokerPlanning.Domain.src.Models.UserAggregate.Enums;

public enum UserRole
{
    [Description("Guest")]
    Guest,
    [Description("Member")]
    Member,
    [Description("Admin")]
    Admin
}
