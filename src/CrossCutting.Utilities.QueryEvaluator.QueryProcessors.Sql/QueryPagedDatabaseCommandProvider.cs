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

    public async Task<Result<IPagedDatabaseCommand>> CreatePagedAsync(IQueryContext source, DatabaseOperation operation, int offset, int pageSize)
        => await (await new AsyncResultDictionaryBuilder()
            .Add(() => Result.Validate(() => operation == DatabaseOperation.Select, "Only select operation is supported"))
            .Add("Settings", () => GetSettings(source))
            .Add("FieldInfo", () => _fieldInfoProvider.Create(source.Query).EnsureValue())
            .Build().ConfigureAwait(false))
            .OnSuccessAsync(results => BuildCommandAsync(
                source,
                offset,
                pageSize,
                results.GetValue<IPagedDatabaseEntityRetrieverSettings>("Settings"),
                results.GetValue<IQueryFieldInfo>("FieldInfo"))).ConfigureAwait(false);

    public Result<IPagedDatabaseEntityRetrieverSettings> Create<TResult>() where TResult : class
        => _settingsProviders
            .Select(x => x.Get<TResult>())
            .WhenNotContinue(() => Result.Invalid<IPagedDatabaseEntityRetrieverSettings>($"No database entity retriever provider was found for query type [{typeof(TResult).FullName}]"));

    private Result<IPagedDatabaseEntityRetrieverSettings> GetSettings(IQueryContext source)
        => Result.WrapException(() =>
        {
            try
            {
                return ((Result<IPagedDatabaseEntityRetrieverSettings>)GetType()
                    .GetMethod(nameof(Create))
                    .MakeGenericMethod(source.Query.GetType())
                    .Invoke(this, Array.Empty<object>())).EnsureValue();
            }
            catch (TargetInvocationException ex)
            {
                return Result.Error<IPagedDatabaseEntityRetrieverSettings>(ex.InnerException, "Could not get paged database entity retriever settings, see exception for details");
            }
        });

    private async Task<Result<IPagedDatabaseCommand>> BuildCommandAsync(IQueryContext source, int offset, int pageSize, IPagedDatabaseEntityRetrieverSettings settings, IQueryFieldInfo fieldInfo)
    {
        var parameterBag = new ParameterBag();
        var builder = new PagedSelectCommandBuilder();

        return (await new AsyncResultDictionaryBuilder()
            .Add(() => builder.Select(source, settings, fieldInfo, _sqlExpressionProvider, parameterBag))
            .Add(() => builder.Distinct(source))
            .Add(() => builder.Top(source, settings, pageSize))
            .Add(() => builder.Offset(source, offset))
            .Add(() => builder.From(source, settings))
            .Add(() => builder.Where(source, settings, fieldInfo, _sqlExpressionProvider, _sqlConditionExpressionProvider, parameterBag))
            .Add(() => builder.OrderBy(source, settings, fieldInfo, _sqlExpressionProvider, parameterBag))
            .Build().ConfigureAwait(false))
            .OnSuccess(_ => builder.AddParameters(source, parameterBag))
            .OnSuccess(_ => builder.Build());
    }
}
