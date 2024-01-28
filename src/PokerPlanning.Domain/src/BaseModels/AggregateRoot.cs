namespace PokerPlanning.Domain.src.BaseModels;

public class AggregateRoot<TId> : Entity<TId>
    where TId : notnull
{
    protected AggregateRoot(TId id) : base(id)
    { }
}
