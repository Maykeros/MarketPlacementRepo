namespace MarketPlacement.Application.Result.Generic;

using Domain.ResultModel.Generic;

public class Result<TData> : IResult<TData>
{
    private readonly List<string> _messagesList;
    
    private Result(bool success,TData data,IEnumerable<string>? messagesList = null, Exception? exception = null)
    {
        Success = success;
        Data = data;
        Exception = exception;
        _messagesList = messagesList?.ToList() ?? new List<string>();
    }
    public bool Success { get; }
    public TData Data { get; }
    
    public IReadOnlyCollection<string> Messages => _messagesList;
    public Exception? Exception { get; }

    public static Result<TData> CreateSuccess(TData data)
    {
        return new(true, data);
    }

    public static Result<TData> CreateFailed(string message, Exception? exception = null)
    {
        return new(false, default!, new List<string> {message}, exception);
    }

    public static Result<TData> CreateFailed(IEnumerable<string> messages, Exception? exception = null)
    {
        return new(false, default!, messages, exception);
    }
    

    public Result<TData> AddError(string message)
    {
        _messagesList.Add(message);

        return this;
    }

    public Result<TData> AddErrors(IEnumerable<string> collection)
    {
        _messagesList.AddRange(collection);

        return this;
    }


}