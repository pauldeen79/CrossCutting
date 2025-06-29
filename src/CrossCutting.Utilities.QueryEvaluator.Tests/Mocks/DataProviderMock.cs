namespace CrossCutting.Utilities.QueryEvaluator.Tests.Mocks;

internal sealed class DataProviderMock : DataProviderBase
{
    public DataProviderMock()
    {
        ResultDelegate = new Func<Query, Task<Result<IEnumerable>>>
        (
            async query =>
            {
                if (SourceData is null) throw new InvalidOperationException("SourceData is not initialized");
                if (CreateContextDelegate is null) throw new InvalidOperationException("CreateContextDelegate is not initialized");

                var results = new List<object>();
                foreach (var item in SourceData)
                {
                    var result = await IsItemValid(query, CreateContextDelegate(item)!).ConfigureAwait(false);

                    if (!result.IsSuccessful())
                    {
                        return Result.FromExistingResult<IEnumerable>(result);
                    }

                    if (result.Value)
                    {
                        results.Add(item);
                    }
                }

                return Result.Success<IEnumerable>(results);
            }
        );
    }

    public object[] SourceData { get; set; } = default!;
    public Func<object, ExpressionEvaluatorContext> CreateContextDelegate { get; set; } = default!;

    private Func<Query, Task<Result<IEnumerable>>> ResultDelegate { get; set; }

    public override async Task<Result<IEnumerable<TResult>>> GetDataAsync<TResult>(Query query)
        where TResult : class
        => (await ResultDelegate.Invoke(query).ConfigureAwait(false))
            .Transform(x => x.Cast<TResult>());
}
