using PokerPlanning.Domain.src.BaseModels;

namespace PokerPlanning.Domain.src.Models.UserAggregate;

public class User : AggregateRoot<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    protected User(
        Guid id,
        string firstName,
        string lastName,
        string email,
        string password) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
    }

    public static User Create(string firstName, string lastName, string email, string password)
    {
        return new(
            Guid.NewGuid(),
            firstName,
            lastName,
            email,
            password
        );
    }
}