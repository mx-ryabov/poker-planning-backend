namespace PokerPlanning.Domain.src.Common.Results;

public class UpdateResultWithData<T> : UpdateResult where T : class
{
    public T? Data { get; private set; }
    protected UpdateResultWithData(bool success) : base(success)
    {
    }

    protected UpdateResultWithData(bool success, List<string> errors) : base(success, errors)
    {
    }

    public static UpdateResultWithData<T> Ok(T data)
    {
        return new(true) { Data = data };
    }

    public static new UpdateResultWithData<T> Error(List<string> errors)
    {
        return new(false, errors) { };
    }
}
