namespace CrossCutting.Data.Core;

public class DatabaseCommandResult<T>(bool success, T? data) : IDatabaseCommandResult<T>
    where T : class
{
    public bool Success { get; } = success;
    public T? Data { get; } = data;

    public DatabaseCommandResult(T? data) : this(data is not null, data)
    {
    }
}
