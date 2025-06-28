namespace CrossCutting.Utilities.QueryEvaluator.Tests.Mocks;

internal sealed class DataProviderMock : IDataProvider
{
    public Func<Query, Task<Result<IEnumerable>>> ResultDelegate { get; set; }
        = new Func<Query, Task<Result<IEnumerable>>>(
            _ => Task.FromResult(Result.Success<IEnumerable>(Enumerable.Empty<object>())));

    public async Task<Result<IEnumerable<TResult>>> GetDataAsync<TResult>(Query query)
        where TResult : class
        => (await ResultDelegate.Invoke(query).ConfigureAwait(false))
            .Transform(x => x.Cast<TResult>());
}
