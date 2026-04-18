namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql;

public class QueryFieldInfoProvider(IEnumerable<IQueryFieldInfoProviderHandler> handlers) : IQueryFieldInfoProvider
{
    private readonly IQueryFieldInfoProviderHandler[] _handlers = ArgumentGuard.IsNotNull(handlers, nameof(handlers)).ToArray();

    public Result<IQueryFieldInfo> Create(IQuery query)
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        return _handlers
            .Select(x => x.Create(query))
            .WhenNotContinue(() => Result.Invalid<IQueryFieldInfo>($"No query field info provider handler found for type: {query.GetType().FullName}"));
    }
}
