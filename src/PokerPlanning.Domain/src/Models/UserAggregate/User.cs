using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Models.UserAggregate.Enums;

namespace PokerPlanning.Domain.src.Models.UserAggregate;

public abstract class User : AggregateRoot<Guid>
{
    public virtual UserRole Role { get; }
    protected User(Guid id) : base(id)
    {
    }
}