namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql;

public class QueryDatabaseCommandProvider : IDatabaseCommandProvider<IQuery>
{
    private readonly IPagedDatabaseCommandProvider<IQueryWrapper> _pagedDatabaseCommandProvider;

    public QueryDatabaseCommandProvider(IPagedDatabaseCommandProvider<IQueryWrapper> pagedDatabaseCommandProvider)
    {
        ArgumentGuard.IsNotNull(pagedDatabaseCommandProvider, nameof(pagedDatabaseCommandProvider));

        _pagedDatabaseCommandProvider = pagedDatabaseCommandProvider;
    }

    public IDatabaseCommand Create(IQuery source, DatabaseOperation operation)
    {
        if (operation != DatabaseOperation.Select)
        {
            throw new ArgumentOutOfRangeException(nameof(operation), "Only select operation is supported");
        }

        return _pagedDatabaseCommandProvider.CreatePaged(source.WithContext(null), operation, 0, 0).DataCommand;
    }
}
