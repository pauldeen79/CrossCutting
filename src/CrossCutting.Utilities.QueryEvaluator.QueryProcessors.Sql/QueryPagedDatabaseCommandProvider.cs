namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql;

public class QueryPagedDatabaseCommandProvider : IPagedDatabaseCommandProvider<IQueryContext>
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

    public Result<IPagedDatabaseCommand> CreatePaged(IQueryContext source, DatabaseOperation operation, int offset, int pageSize)
    {
        source = ArgumentGuard.IsNotNull(source, nameof(source));

        if (operation != DatabaseOperation.Select)
        {
            return Result.Invalid<IPagedDatabaseCommand>("Only select operation is supported");
        }

        Result<IPagedDatabaseEntityRetrieverSettings> settingsResult;
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            settingsResult = (Result<IPagedDatabaseEntityRetrieverSettings>)GetType()
                .GetMethod(nameof(Create))
                .MakeGenericMethod(source.Query.GetType())
                .Invoke(this, Array.Empty<object>());
        }
        catch (TargetInvocationException ex)
        {
            return Result.Error<IPagedDatabaseCommand>(ex.InnerException, "Could not get paged database entity retriever settings, see exception for details");
        }
        catch (Exception ex)
        {
            return Result.Error<IPagedDatabaseCommand>(ex, "Could not get paged database entity retriever settings, see exception for details");
        }
#pragma warning restore CA1031 // Do not catch general exception types

        if (!settingsResult.IsSuccessful())
        {
            return Result.FromExistingResult<IPagedDatabaseCommand>(settingsResult);
        }

        var settings = settingsResult.Value!;
        var fieldInfo = _fieldInfoProvider.Create(source.Query).GetValueOrThrow();
        var parameterBag = new ParameterBag();
        return new PagedSelectCommandBuilder()
            .Select(source, settings, fieldInfo, _sqlExpressionProvider, parameterBag)
            .OnSuccess(result => result.Distinct(source))
            .OnSuccess(result => result.Top(source, settings, pageSize))
            .OnSuccess(result => result.Offset(source, offset))
            .OnSuccess(result => result.From(source, settings))
            .OnSuccess(result => result.Where(source, settings, fieldInfo, _sqlExpressionProvider, _sqlConditionExpressionProvider, parameterBag))
            .OnSuccess(result => result.OrderBy(source, settings, fieldInfo, _sqlExpressionProvider, parameterBag))
            .OnSuccess(result => result.WithParameters(source, parameterBag))
            .OnSuccess(result => result.Build());
    }

    public Result<IPagedDatabaseEntityRetrieverSettings> Create<TResult>() where TResult : class
        => _settingsProviders
            .Select(x => x.Get<TResult>())
            .WhenNotContinue(() => Result.Invalid<IPagedDatabaseEntityRetrieverSettings>($"No database entity retriever provider was found for query type [{typeof(TResult).FullName}]"));
}
