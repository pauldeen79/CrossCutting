namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql;

public class QueryFieldInfoProvider : IQueryFieldInfoProvider
{
    private readonly IEnumerable<IQueryFieldInfoProviderHandler> _handlers;

    public QueryFieldInfoProvider(IEnumerable<IQueryFieldInfoProviderHandler> handlers)
    {
        ArgumentGuard.IsNotNull(handlers, nameof(handlers));

        _handlers = handlers;
    }

    public Result<IQueryFieldInfo> Create(IQuery query)
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        return _handlers
            .Select(x => x.Create(query))
            .WhenNotContinue(() => Result.Invalid<IQueryFieldInfo>($"No query field info provider handler found for type: {query.GetType().FullName}"));
    }
}
