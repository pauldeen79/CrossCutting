namespace CrossCutting.Utilities.QueryEvaluator.Tests.Mocks;

internal sealed class ContextDataProviderMock : IContextDataProvider
{
    public Func<Query, object?, Task<Result<IEnumerable>>> ContextResultDelegate { get; set; }
        = new Func<Query, object?, Task<Result<IEnumerable>>>((_, _) => Task.FromResult(Result.Success<IEnumerable>(Enumerable.Empty<object>())));

    public Task<Result<IEnumerable<TResult>>> GetDataAsync<TResult>(Query query)
        where TResult : class
        => GetDataAsync<TResult>(query, default);

    public async Task<Result<IEnumerable<TResult>>> GetDataAsync<TResult>(Query query, object? context)
        where TResult : class
        => (await ContextResultDelegate.Invoke(query, context).ConfigureAwait(false)).Transform(x => x.Cast<TResult>());
}
