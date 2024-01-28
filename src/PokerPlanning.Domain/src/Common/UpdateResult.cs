namespace PokerPlanning.Domain.src.Common;

public abstract class Result
{
    public bool Success { get; private set; }
    public List<string> Errors { get; private set; } = new List<string>();

    protected Result(bool success)
    {
        Success = success;
    }

    protected Result(bool success, List<string> errors)
    {
        Success = success;
        Errors = errors;
    }
}

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
