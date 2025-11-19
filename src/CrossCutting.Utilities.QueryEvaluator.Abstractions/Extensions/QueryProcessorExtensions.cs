namespace CrossCutting.Utilities.QueryEvaluator.Abstractions.Extensions;

public static class QueryProcessorExtensions
{
    public static Task<Result<TResult>> FindOneAsync<TResult>(this IQueryProcessor processor, IQuery query, CancellationToken token)
        where TResult : class
        => processor.FindOneAsync<TResult>(query, null, token);

    public static Task<Result<TResult>> FindOneAsync<TResult>(this IQueryProcessor processor, IQuery query, object? context)
        where TResult : class
        => processor.FindOneAsync<TResult>(query, context, CancellationToken.None);

    public static Task<Result<TResult>> FindOneAsync<TResult>(this IQueryProcessor processor, IQuery query)
        where TResult : class
        => processor.FindOneAsync<TResult>(query, null, CancellationToken.None);

    public static Task<Result<IReadOnlyCollection<TResult>>> FindManyAsync<TResult>(this IQueryProcessor processor, IQuery query, CancellationToken token)
        where TResult : class
        => processor.FindManyAsync<TResult>(query, null, token);

    public static Task<Result<IReadOnlyCollection<TResult>>> FindManyAsync<TResult>(this IQueryProcessor processor, IQuery query, object? context)
        where TResult : class
        => processor.FindManyAsync<TResult>(query, context, CancellationToken.None);

    public static Task<Result<IReadOnlyCollection<TResult>>> FindManyAsync<TResult>(this IQueryProcessor processor, IQuery query)
        where TResult : class
        => processor.FindManyAsync<TResult>(query, null, CancellationToken.None);

    public static Task<Result<IPagedResult<TResult>>> FindPagedAsync<TResult>(this IQueryProcessor processor, IQuery query, CancellationToken token)
        where TResult : class
        => processor.FindPagedAsync<TResult>(query, null, token);

    public static Task<Result<IPagedResult<TResult>>> FindPagedAsync<TResult>(this IQueryProcessor processor, IQuery query, object? context)
        where TResult : class
        => processor.FindPagedAsync<TResult>(query, context, CancellationToken.None);

    public static Task<Result<IPagedResult<TResult>>> FindPagedAsync<TResult>(this IQueryProcessor processor, IQuery query)
        where TResult : class
        => processor.FindPagedAsync<TResult>(query, null, CancellationToken.None);
}
