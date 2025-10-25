namespace CrossCutting.Common.Abstractions;

public interface ITaskDecorator
{
    Task<Result> Execute(KeyValuePair<string, Func<Task<Result>>> taskDelegateItem);
}

public interface ITaskDecorator<T>
{
    Task<Result<T>> Execute(KeyValuePair<string, Func<Task<Result<T>>>> taskDelegateItem);
}
