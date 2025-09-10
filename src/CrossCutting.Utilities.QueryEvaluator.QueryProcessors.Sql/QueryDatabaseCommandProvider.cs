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
        => Result.Validate(() => operation == DatabaseOperation.Select, "Only select operation is supported")
            .TryCastAllowNull<IDatabaseCommand>()
            .OnSuccess(() => _pagedDatabaseCommandProvider.CreatePaged(source.WithContext(null), operation, 0, 0)
                .EnsureValue()
                .OnSuccess(pagedDatabaseCommand => pagedDatabaseCommand.DataCommand));
}
