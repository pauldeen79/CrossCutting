namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql;

public class QueryPagedDatabaseCommandProvider(IEntityFieldInfoProvider fieldInfoProvider,
                                               ISqlExpressionProvider sqlExpressionProvider,
                                               ISqlConditionExpressionProvider sqlConditionExpressionProvider,
                                               IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> settingsProviders) : IPagedDatabaseCommandProvider<IQueryContext>
{
    private readonly IEntityFieldInfoProvider _fieldInfoProvider = ArgumentGuard.IsNotNull(fieldInfoProvider, nameof(fieldInfoProvider));
    private readonly ISqlExpressionProvider _sqlExpressionProvider = ArgumentGuard.IsNotNull(sqlExpressionProvider, nameof(sqlExpressionProvider));
    private readonly ISqlConditionExpressionProvider _sqlConditionExpressionProvider = ArgumentGuard.IsNotNull(sqlConditionExpressionProvider, nameof(sqlConditionExpressionProvider));
    private readonly IPagedDatabaseEntityRetrieverSettingsProvider[] _settingsProviders = ArgumentGuard.IsNotNull(settingsProviders, nameof(settingsProviders)).ToArray();

    public async Task<Result<IPagedDatabaseCommand>> CreatePagedAsync(IQueryContext context, DatabaseOperation operation, int offset, int pageSize, CancellationToken token)
        => await (await new AsyncResultDictionaryBuilder()
            .Add(() => Result.Validate(() => operation == DatabaseOperation.Select, "Only select operation is supported"))
            .Add("Settings", () => GetSettings(context.Query.GetType()))
            .Add("FieldInfo", () => _fieldInfoProvider.Create(context.Query).EnsureValue())
            .BuildAsync(token).ConfigureAwait(false))
            .OnSuccessAsync(results => BuildCommandAsync(context, offset, pageSize, results.GetValue<IPagedDatabaseEntityRetrieverSettings>("Settings"), results.GetValue<IEntityFieldInfo>("FieldInfo"), token)).ConfigureAwait(false);

    public Result<IPagedDatabaseEntityRetrieverSettings> Create<TResult>() where TResult : class
        => _settingsProviders
            .Select(x => x.Get<TResult>())
            .WhenNotContinue(() => Result.Invalid<IPagedDatabaseEntityRetrieverSettings>($"No database entity retriever settings provider was found for query type [{typeof(TResult).FullName}]"));

    private Result<IPagedDatabaseEntityRetrieverSettings> GetSettings(Type queryType)
        => Result.WrapException(() =>
        {
            try
            {
                return ((Result<IPagedDatabaseEntityRetrieverSettings>)GetType()
                    .GetMethod(nameof(Create))
                    .MakeGenericMethod(queryType)
                    .Invoke(this, Array.Empty<object>())).EnsureValue();
            }
            catch (TargetInvocationException ex)
            {
                return Result.Error<IPagedDatabaseEntityRetrieverSettings>(ex.InnerException, "Could not get paged database entity retriever settings, see exception for details");
            }
        });

    private async Task<Result<IPagedDatabaseCommand>> BuildCommandAsync(IQueryContext context, int offset, int pageSize, IPagedDatabaseEntityRetrieverSettings settings, IEntityFieldInfo fieldInfo, CancellationToken token)
    {
        var parameterBag = new ParameterBag();
        var builder = new PagedSelectCommandBuilder();
        var builderContext = new PagedSelectCommandBuilderContext(context, settings, fieldInfo, _sqlExpressionProvider, parameterBag);

        return (await new AsyncResultDictionaryBuilder()
            .Add(() => builder.Select(builderContext))
            .Add(() => builder.Distinct(context))
            .Add(() => builder.Top(context, settings, pageSize))
            .Add(() => builder.Offset(context, offset))
            .Add(() => builder.From(context, settings))
            .Add(() => builder.Where(builderContext, _sqlConditionExpressionProvider, token))
            .Add(() => builder.OrderBy(builderContext, token))
            .BuildAsync(token).ConfigureAwait(false))
            .OnSuccess(_ => builder.AddParameters(context, parameterBag))
            .OnSuccess(_ => builder.Build());
    }
}
