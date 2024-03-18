namespace PokerPlanning.Domain.src.Common.Results;

public abstract class Result
{
    public bool Success { get; private set; }
    public List<string> Errors { get; private set; } = new List<string>();

    public string ErrorsString => string.Join("; ", Errors);

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
