namespace CrossCutting.Common.Abstractions;

public interface ITaskDecorator
{
    Task<Result> Execute(KeyValuePair<string, Task<Result>> taskItem);
}

public interface ITaskDecorator<T>
{
    Task<Result<T>> Execute(KeyValuePair<string, Task<Result<T>>> taskItem);
}
