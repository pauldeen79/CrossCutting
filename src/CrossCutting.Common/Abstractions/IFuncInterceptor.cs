namespace CrossCutting.Common.Abstractions;

public interface IFuncInterceptor
{
    Result Execute(KeyValuePair<string, Func<Result>> item, Func<Result> next);
}

public interface IFuncInterceptor<T>
{
    Result<T> Execute(KeyValuePair<string, Func<Result<T>>> item, Func<Result<T>> next);
}
