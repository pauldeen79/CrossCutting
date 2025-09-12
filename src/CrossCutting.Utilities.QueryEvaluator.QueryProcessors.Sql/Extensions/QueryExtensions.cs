namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Extensions;

public static class QueryExtensions
{
    public static IQueryContext WithContext(this IQuery query, object? context)
        => new QueryWrapper(query, context);

    private sealed class QueryWrapper : IQueryContext
    {
        public QueryWrapper(IQuery query, object? context)
        {
            Query = query;
            Context = context;
        }

        public IQuery Query { get; }

        public object? Context { get; }
    }
}
