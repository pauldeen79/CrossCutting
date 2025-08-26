namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql;

public class QueryPagedDatabaseCommandProvider : IPagedDatabaseCommandProvider<IQuery>
{
    private readonly IQueryFieldInfoFactory _fieldInfoFactory;
    private readonly ISqlExpressionProvider _sqlExpressionProvider;
    private readonly IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> _settingsProviders;

    public QueryPagedDatabaseCommandProvider(IQueryFieldInfoFactory fieldInfoFactory,
                                             ISqlExpressionProvider sqlExpressionProvider,
                                             IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> settingsProviders)
    {
        ArgumentGuard.IsNotNull(fieldInfoFactory, nameof(fieldInfoFactory));
        ArgumentGuard.IsNotNull(sqlExpressionProvider, nameof(sqlExpressionProvider));
        ArgumentGuard.IsNotNull(settingsProviders, nameof(settingsProviders));

        _fieldInfoFactory = fieldInfoFactory;
        _sqlExpressionProvider = sqlExpressionProvider;
        _settingsProviders = settingsProviders;
    }

    public IPagedDatabaseCommand CreatePaged(IQuery source, DatabaseOperation operation, int offset, int pageSize)
    {
        source = ArgumentGuard.IsNotNull(source, nameof(source));

        if (operation != DatabaseOperation.Select)
        {
            throw new ArgumentOutOfRangeException(nameof(operation), "Only select operation is supported");
        }

        var fieldSelectionQuery = source as IFieldSelectionQuery;
        var parameterizedQuery = source as IParameterizedQuery;
        IPagedDatabaseEntityRetrieverSettings settings;
        try
        {
            settings = (IPagedDatabaseEntityRetrieverSettings)GetType()
                .GetMethod(nameof(Create))
                .MakeGenericMethod(source.GetType())
                .Invoke(this, Array.Empty<object>());
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException;
        }
        var fieldInfo = _fieldInfoFactory.Create(source);
        var parameterBag = new ParameterBag();
        return new PagedSelectCommandBuilder()
            .Select(settings, fieldInfo, fieldSelectionQuery, _sqlExpressionProvider, parameterBag)
            .Distinct(fieldSelectionQuery)
            .Top(source, settings)
            .Offset(source)
            .From(source, settings)
            .Where(source, settings, fieldInfo, _sqlExpressionProvider, parameterBag)
            .OrderBy(source, settings, fieldInfo, _sqlExpressionProvider, parameterBag)
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
