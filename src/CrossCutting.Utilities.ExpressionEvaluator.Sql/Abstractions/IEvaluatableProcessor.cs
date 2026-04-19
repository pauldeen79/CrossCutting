namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IEvaluatableProcessor
{
    Task<Result<TResult>> FindOneAsync<TResult>(IEvaluatable<bool> evaluatable, object? context, CancellationToken token)
        where TResult : class;
    Task<Result<IReadOnlyCollection<TResult>>> FindManyAsync<TResult>(IEvaluatable<bool> evaluatable, object? context, CancellationToken token)
        where TResult : class;
    Task<Result<IPagedResult<TResult>>> FindPagedAsync<TResult>(IEvaluatable<bool> evaluatable, int offset, int pageSize, object? context, CancellationToken token)
        where TResult : class;
}
