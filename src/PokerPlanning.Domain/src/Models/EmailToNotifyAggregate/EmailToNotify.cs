using PokerPlanning.Domain.src.BaseModels;

namespace PokerPlanning.Domain.src.Models.EmailToNotifyAggregate;

public class EmailToNotify : AggregateRoot<Guid>
{
    protected EmailToNotify(Guid id) : base(id)
    {
    }

    public required string Email { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public static EmailToNotify Create(string email)
    {
        return new(Guid.NewGuid())
        {
            Email = email,
        }; 
    }
}
