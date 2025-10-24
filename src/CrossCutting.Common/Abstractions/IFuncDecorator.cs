namespace CrossCutting.Common.Abstractions;

public interface IFuncDecorator
{
    Result Execute(KeyValuePair<string, Func<Result>> taskItem);
}

public interface IFuncDecorator<T>
{
    Result<T> Execute(KeyValuePair<string, Func<Result<T>>> taskItem);
}
