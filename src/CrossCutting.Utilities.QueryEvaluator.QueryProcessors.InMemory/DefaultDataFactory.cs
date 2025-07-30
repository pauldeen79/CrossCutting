namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.InMemory;

public class DefaultDataFactory : IDataFactory
{
    private readonly IEnumerable<IDataProvider> _providers;

    public DefaultDataFactory(IEnumerable<IDataProvider> providers)
    {
        ArgumentGuard.IsNotNull(providers, nameof(providers));

        _providers = providers;
    }

    public async Task<Result<IEnumerable<TResult>>> GetDataAsync<TResult>(IQuery query)
        where TResult : class
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

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
