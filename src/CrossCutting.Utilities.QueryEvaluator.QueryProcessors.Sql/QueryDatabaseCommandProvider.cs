namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql;

public class QueryDatabaseCommandProvider : IDatabaseCommandProvider<IQuery>
{
    private readonly IPagedDatabaseCommandProvider<IQueryContext> _pagedDatabaseCommandProvider;

    public QueryDatabaseCommandProvider(IPagedDatabaseCommandProvider<IQueryContext> pagedDatabaseCommandProvider)
    {
        ArgumentGuard.IsNotNull(pagedDatabaseCommandProvider, nameof(pagedDatabaseCommandProvider));

        _pagedDatabaseCommandProvider = pagedDatabaseCommandProvider;
    }

    public Result<IDatabaseCommand> Create(IQuery source, DatabaseOperation operation)
        => new ResultDictionaryBuilder()
            .Add("Validate", () => Result.Validate(() => operation == DatabaseOperation.Select, "Only select operation is supported"))
            .Add("Command", () => _pagedDatabaseCommandProvider.CreatePaged(source.WithContext(null), operation, 0, 0).EnsureValue())
            .Build()
            .OnSuccess(results => results.GetValue<IPagedDatabaseCommand>("Command").DataCommand);
}
