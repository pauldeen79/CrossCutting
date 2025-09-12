namespace CrossCutting.Utilities.QueryEvaluator.Abstractions.Extensions;

public static class QueryProcessorExtensions
{
    public static Task<Result<TResult>> FindOneAsync<TResult>(this IQueryProcessor processor, IQuery query, CancellationToken cancellationToken)
        where TResult : class
        => processor.FindOneAsync<TResult>(query, null, cancellationToken);

    public static Task<Result<TResult>> FindOneAsync<TResult>(this IQueryProcessor processor, IQuery query, object? context)
        where TResult : class
        => processor.FindOneAsync<TResult>(query, context, CancellationToken.None);

    public static Task<Result<TResult>> FindOneAsync<TResult>(this IQueryProcessor processor, IQuery query)
        where TResult : class
        => processor.FindOneAsync<TResult>(query, null, CancellationToken.None);

    public static Task<Result<IReadOnlyCollection<TResult>>> FindManyAsync<TResult>(this IQueryProcessor processor, IQuery query, CancellationToken cancellationToken)
        where TResult : class
        => processor.FindManyAsync<TResult>(query, null, cancellationToken);

    public static Task<Result<IReadOnlyCollection<TResult>>> FindManyAsync<TResult>(this IQueryProcessor processor, IQuery query, object? context)
        where TResult : class
        => processor.FindManyAsync<TResult>(query, context, CancellationToken.None);

    public static Task<Result<IReadOnlyCollection<TResult>>> FindManyAsync<TResult>(this IQueryProcessor processor, IQuery query)
        where TResult : class
        => processor.FindManyAsync<TResult>(query, null, CancellationToken.None);

    public static Task<Result<IPagedResult<TResult>>> FindPagedAsync<TResult>(this IQueryProcessor processor, IQuery query, CancellationToken cancellationToken)
        where TResult : class
        => processor.FindPagedAsync<TResult>(query, null, cancellationToken);

    public static Task<Result<IPagedResult<TResult>>> FindPagedAsync<TResult>(this IQueryProcessor processor, IQuery query, object? context)
        where TResult : class
        => processor.FindPagedAsync<TResult>(query, context, CancellationToken.None);

    public static Task<Result<IPagedResult<TResult>>> FindPagedAsync<TResult>(this IQueryProcessor processor, IQuery query)
        where TResult : class
        => processor.FindPagedAsync<TResult>(query, null, CancellationToken.None);
}
