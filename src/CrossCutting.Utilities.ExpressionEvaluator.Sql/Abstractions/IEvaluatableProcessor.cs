namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IEvaluatableProcessor
{
    Task<Result<TResult>> FindOneAsync<TResult>(IEvaluatable<bool> evaluatable, IEvaluatable? orderByEvaluatable, object? context, CancellationToken token)
        where TResult : class;
    Task<Result<IReadOnlyCollection<TResult>>> FindManyAsync<TResult>(IEvaluatable<bool> evaluatable, IEvaluatable? orderByEvaluatable, object? context, CancellationToken token)
        where TResult : class;
    Task<Result<IPagedResult<TResult>>> FindPagedAsync<TResult>(IEvaluatable<bool> evaluatable, int offset, int pageSize, IEvaluatable? orderByEvaluatable, object? context, CancellationToken token)
        where TResult : class;
}
