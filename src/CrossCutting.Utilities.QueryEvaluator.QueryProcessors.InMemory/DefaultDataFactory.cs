namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.InMemory;

public class DefaultDataFactory(IEnumerable<IDataProvider> providers) : IDataFactory
{
    private readonly IDataProvider[] _providers = ArgumentGuard.IsNotNull(providers, nameof(providers)).ToArray();

    public async Task<Result<IEnumerable<TResult>>> GetDataAsync<TResult>(IQuery query, object? context)
        where TResult : class
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        foreach (var provider in _providers)
        {
            var result = await provider.GetDataAsync<TResult>(query, context).ConfigureAwait(false);

            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        return Result.Error<IEnumerable<TResult>>($"Query type [{query.GetType().FullName}] for data type [{typeof(TResult).FullName}] does not have a data provider");
    }
}
