namespace MarketPlacement.Application.Result;

using Domain.ResultModel;

public class Result : IResult
{
    private readonly List<string> _messagesList;

    private Result(bool success, IEnumerable<string>? messagesList = null, Exception? exception = null)
    {
        Success = success;
        Exception = exception;
        _messagesList = messagesList?.ToList() ?? new List<string>();

    }

    public IReadOnlyCollection<string> Messages => _messagesList;
    public bool Success { get; }
    public Exception? Exception { get; }
    
    public static Result CreateSuccess()
    {
        return new(true);
    }

    public static Result CreateFailed(string message, Exception? exception = null)
    {
        return new(false, new List<string> {message}, exception);
    }

    public static Result CreateFailed(IEnumerable<string> messages, Exception? exception = null)
    {
        return new(false, messages, exception);
    }
    

    public Result AddError(string message)
    {
        _messagesList.Add(message);

        return this;
    }

    public Result AddErrors(IEnumerable<string> collection)
    {
        _messagesList.AddRange(collection);

        return this;
    }
}