namespace CrossCutting.Data.Core;

public class DatabaseCommandResult<T> : IDatabaseCommandResult<T>
    where T : class
{
    public bool Success { get; }
    public T? Data { get; }

    public DatabaseCommandResult(T? data) : this(data is not null, data)
    {
    }

    public DatabaseCommandResult(bool success, T? data)
    {
        Success = success;
        Data = data;
    }
}
