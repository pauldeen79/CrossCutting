namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.InMemory;

public class DefaultDataFactory : IContextDataFactory
{
    private readonly IEnumerable<IDataProvider> _providers;
    private readonly IEnumerable<IContextDataProvider> _contextProviders;

    public DefaultDataFactory(IEnumerable<IDataProvider> providers, IEnumerable<IContextDataProvider> contextProviders)
    {
        _providers = providers;
        _contextProviders = contextProviders;
    }

    public Task<Result<IEnumerable<TResult>>> GetDataAsync<TResult>(IQuery query)
        where TResult : class
        => GetDataAsync<TResult>(query, default);

    public async Task<Result<IEnumerable<TResult>>> GetDataAsync<TResult>(IQuery query, object? context)
        where TResult : class
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        foreach (var provider in _contextProviders)
        {
            var result = await provider.GetDataAsync<TResult>(query, context).ConfigureAwait(false);

            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        foreach (var provider in _providers)
        {
            var result = await provider.GetDataAsync<TResult>(query).ConfigureAwait(false);

            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        return Result.Error<IEnumerable<TResult>>($"Query type [{query.GetType().FullName}] for data type [{typeof(TResult).FullName}] does not have a data provider");
    }
}
