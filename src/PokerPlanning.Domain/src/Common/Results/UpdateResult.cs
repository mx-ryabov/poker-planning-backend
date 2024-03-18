namespace PokerPlanning.Domain.src.Common.Results;

public class UpdateResult : Result
{
    protected UpdateResult(bool success) : base(success)
    {
    }

    protected UpdateResult(bool success, List<string> errors) : base(success, errors)
    {
    }

    public static UpdateResult Ok()
    {
        return new(true) { };
    }

    public static UpdateResult Error(List<string> errors)
    {
        return new(false, errors) { };
    }
}
