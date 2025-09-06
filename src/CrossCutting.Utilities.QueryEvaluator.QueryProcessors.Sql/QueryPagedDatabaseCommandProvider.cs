namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql;

public class QueryPagedDatabaseCommandProvider : IPagedDatabaseCommandProvider<IQueryWrapper>
{
    private readonly IQueryFieldInfoProvider _fieldInfoProvider;
    private readonly ISqlExpressionProvider _sqlExpressionProvider;
    private readonly ISqlConditionExpressionProvider _sqlConditionExpressionProvider;
    private readonly IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> _settingsProviders;

    public QueryPagedDatabaseCommandProvider(IQueryFieldInfoProvider fieldInfoProvider,
                                             ISqlExpressionProvider sqlExpressionProvider,
                                             ISqlConditionExpressionProvider sqlConditionExpressionProvider,
                                             IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> settingsProviders)
    {
        ArgumentGuard.IsNotNull(fieldInfoProvider, nameof(fieldInfoProvider));
        ArgumentGuard.IsNotNull(sqlExpressionProvider, nameof(sqlExpressionProvider));
        ArgumentGuard.IsNotNull(sqlConditionExpressionProvider, nameof(sqlConditionExpressionProvider));
        ArgumentGuard.IsNotNull(settingsProviders, nameof(settingsProviders));

        _fieldInfoProvider = fieldInfoProvider;
        _sqlExpressionProvider = sqlExpressionProvider;
        _sqlConditionExpressionProvider = sqlConditionExpressionProvider;
        _settingsProviders = settingsProviders;
    }

    public IPagedDatabaseCommand CreatePaged(IQueryWrapper source, DatabaseOperation operation, int offset, int pageSize)
    {
        source = ArgumentGuard.IsNotNull(source, nameof(source));

        if (operation != DatabaseOperation.Select)
        {
            throw new ArgumentOutOfRangeException(nameof(operation), "Only select operation is supported");
        }

        var fieldSelectionQuery = source.Query as IFieldSelectionQuery;
        var parameterizedQuery = source.Query as IParameterizedQuery;
        IPagedDatabaseEntityRetrieverSettings settings;
        try
        {
            settings = (IPagedDatabaseEntityRetrieverSettings)GetType()
                .GetMethod(nameof(Create))
                .MakeGenericMethod(source.Query.GetType())
                .Invoke(this, Array.Empty<object>());
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException;
        }
        var fieldInfo = _fieldInfoProvider.Create(source.Query).GetValueOrThrow();
        var parameterBag = new ParameterBag();
        return new PagedSelectCommandBuilder()
            .Select(source.Context, settings, fieldInfo, fieldSelectionQuery, _sqlExpressionProvider, parameterBag)
            .Distinct(fieldSelectionQuery)
            .Top(source.Query, settings, pageSize)
            .Offset(source.Query, offset)
            .From(source.Query, settings)
            .Where(source.Query, source.Context, settings, fieldInfo, _sqlExpressionProvider, _sqlConditionExpressionProvider, parameterBag)
            .OrderBy(source.Query, source.Context, settings, fieldInfo, _sqlExpressionProvider, parameterBag)
            .WithParameters(parameterizedQuery, parameterBag)
            .Build();
    }

    public IPagedDatabaseEntityRetrieverSettings Create<TResult>() where TResult : class
    {
        foreach (var provider in _settingsProviders)
        {
            var success = provider.TryGet<TResult>(out var result);
            if (success)
            {
                return result ?? throw new InvalidOperationException($"Database entity retriever provider for query type [{typeof(TResult).FullName}] provided an empty result");
            }
        }

        throw new InvalidOperationException($"No database entity retriever provider was found for query type [{typeof(TResult).FullName}]");
    }
}
