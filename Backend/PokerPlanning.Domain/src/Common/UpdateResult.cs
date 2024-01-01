namespace PokerPlanning.Domain.src.Common;

public class UpdateResult
{
    public bool Success { get; set; }
    public List<string> Errors { get; set; } = new List<string>();

    public UpdateResult(bool success)
    {
        Success = success;
    }

    public UpdateResult(bool success, List<string> errors)
    {
        Success = success;
        Errors = errors;
    }
}
