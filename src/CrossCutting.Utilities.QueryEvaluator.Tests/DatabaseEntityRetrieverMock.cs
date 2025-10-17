namespace CrossCutting.Utilities.QueryEvaluator.Tests;

public class DatabaseEntityRetrieverMock<T> : IDatabaseEntityRetriever<T> where T : class
{
    public IDatabaseCommand? LastDatabaseCommand { get; private set; }
    public IPagedDatabaseCommand? LastPagedDatabaseCommand { get; private set; }
    public IEnumerable<object> Data { get; private set; } = default!;
    public IPagedResult<T> PagedResult { get; private set; } = default!;

    public Task<Result<IReadOnlyCollection<T>>> FindManyAsync(IDatabaseCommand command, CancellationToken cancellationToken)
        => Task.Run(() =>
        {
            LastDatabaseCommand = command;
            return Result.Success<IReadOnlyCollection<T>>(Data.OfType<T>().ToList());
        });

    public Task<Result<T>> FindOneAsync(IDatabaseCommand command, CancellationToken cancellationToken)
        => Task.Run(() =>
        {
            LastDatabaseCommand = command;
            return Data.OfType<T>().Any()
                ? Result.Success(Data.OfType<T>().FirstOrDefault()!)
                : Result.NotFound<T>();
        });

    public Task<Result<IPagedResult<T>>> FindPagedAsync(IPagedDatabaseCommand command, CancellationToken cancellationToken)
        => Task.Run(() =>
        {
            LastPagedDatabaseCommand = command;
            return Task.FromResult(Result.Success(PagedResult));
        });

    public void SetData(IEnumerable items, IPagedResult<T> pagedResult)
    {
        PagedResult = pagedResult;
        Data = items.Cast<object>().ToArray();
        PagedResult.Count.Returns(Data.OfType<T>().Count());
        PagedResult.TotalRecordCount.Returns(Data.OfType<T>().Count());
        PagedResult.PageSize.Returns(Data.OfType<T>().Count());
        PagedResult.GetEnumerator().Returns(Data.OfType<T>().GetEnumerator());
    }
}
