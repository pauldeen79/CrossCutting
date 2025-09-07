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
    {
        if (operation != DatabaseOperation.Select)
        {
            return Result.Invalid<IDatabaseCommand>("Only select operation is supported");
        }

        var result = _pagedDatabaseCommandProvider.CreatePaged(source.WithContext(null), operation, 0, 0).EnsureValue();
        if (!result.IsSuccessful())
        {
            return Result.FromExistingResult<IDatabaseCommand>(result);
        }

        return Result.Success(result.Value!.DataCommand);
    }
}
