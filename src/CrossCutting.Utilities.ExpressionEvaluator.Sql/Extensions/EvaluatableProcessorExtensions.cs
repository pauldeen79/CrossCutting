namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.Extensions;

public static class EvaluatableProcessorExtensions
{
    public static Task<Result<TResult>> FindOneAsync<TResult>(this IEvaluatableProcessor instance, IEvaluatable<bool> evaluatable, CancellationToken token)
        where TResult : class
        => instance.FindOneAsync<TResult>(evaluatable, null, null, token);

    public static Task<Result<TResult>> FindOneAsync<TResult>(this IEvaluatableProcessor instance, IEvaluatable<bool> evaluatable, IEvaluatable? orderByEvaluatable, CancellationToken token)
        where TResult : class
        => instance.FindOneAsync<TResult>(evaluatable, orderByEvaluatable, null, token);

    public static Task<Result<TResult>> FindOneAsync<TResult>(this IEvaluatableProcessor instance, IEvaluatable<bool> evaluatable, object? context, CancellationToken token)
        where TResult : class
        => instance.FindOneAsync<TResult>(evaluatable, null, context, token);

    public static Task<Result<IReadOnlyCollection<TResult>>> FindManyAsync<TResult>(this IEvaluatableProcessor instance, IEvaluatable<bool> evaluatable, CancellationToken token)
        where TResult : class
        => instance.FindManyAsync<TResult>(evaluatable, null, null, token);

    public static Task<Result<IReadOnlyCollection<TResult>>> FindManyAsync<TResult>(this IEvaluatableProcessor instance, IEvaluatable<bool> evaluatable, IEvaluatable? orderByEvaluatable, CancellationToken token)
        where TResult : class
        => instance.FindManyAsync<TResult>(evaluatable, orderByEvaluatable, null, token);

    public static Task<Result<IReadOnlyCollection<TResult>>> FindManyAsync<TResult>(this IEvaluatableProcessor instance, IEvaluatable<bool> evaluatable, object? context, CancellationToken token)
        where TResult : class
        => instance.FindManyAsync<TResult>(evaluatable, null, context, token);

    public static Task<Result<IPagedResult<TResult>>> FindPagedAsync<TResult>(this IEvaluatableProcessor instance, IEvaluatable<bool> evaluatable, int offset, int pageSize, CancellationToken token)
        where TResult : class
        => instance.FindPagedAsync<TResult>(evaluatable, offset, pageSize, null, null, token);

    public static Task<Result<IPagedResult<TResult>>> FindPagedAsync<TResult>(this IEvaluatableProcessor instance, IEvaluatable<bool> evaluatable, int offset, int pageSize, IEvaluatable? orderByEvaluatable, CancellationToken token)
        where TResult : class
        => instance.FindPagedAsync<TResult>(evaluatable, offset, pageSize, orderByEvaluatable, null, token);

    public static Task<Result<IPagedResult<TResult>>> FindPagedAsync<TResult>(this IEvaluatableProcessor instance, IEvaluatable<bool> evaluatable, int offset, int pageSize, object? context, CancellationToken token)
        where TResult : class
        => instance.FindPagedAsync<TResult>(evaluatable, offset, pageSize, null, context, token);
}