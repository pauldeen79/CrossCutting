namespace CrossCutting.Common.Abstractions;

public interface ITaskInterceptor
{
    Task<Result> ExecuteAsync(KeyValuePair<string, Func<Task<Result>>> taskDelegateItem, Func<Task<Result>> next, CancellationToken token);
}

public interface ITaskInterceptor<T>
{
    Task<Result<T>> ExecuteAsync(KeyValuePair<string, Func<Task<Result<T>>>> taskDelegateItem, Func<Task<Result<T>>> next, CancellationToken token);
}
