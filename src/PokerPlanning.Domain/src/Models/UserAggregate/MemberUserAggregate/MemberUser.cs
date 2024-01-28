using PokerPlanning.Domain.src.Models.UserAggregate.Enums;

namespace PokerPlanning.Domain.src.Models.UserAggregate.MemberUserAggregate;

public class MemberUser : User
{
    public override UserRole Role { get; } = UserRole.Member;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    protected MemberUser(
        Guid id,
        string email,
        string firstName,
        string lastName) : base(id)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }

    public static MemberUser Create(string email, string firstName, string lastName)
    {
        return Create(
            Guid.NewGuid(),
            email,
            firstName,
            lastName
        );
    }

    public static MemberUser Create(Guid id, string email, string firstName, string lastName)
    {
        return new(
            id,
            email,
            firstName,
            lastName
        );
    }
}
