namespace CrossCutting.Utilities.ExpressionEvaluator.Sql;

public class EvaluatablePagedDatabaseCommandProvider(IEntityFieldInfoProvider fieldInfoProvider,
                                                     IEvaluatableSqlExpressionProvider evaluatableSqlExpressionProvider,
                                                     IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> settingsProviders) : IPagedDatabaseCommandProvider<IEvaluatableContext>
{
    private readonly IEntityFieldInfoProvider _fieldInfoProvider = ArgumentGuard.IsNotNull(fieldInfoProvider, nameof(fieldInfoProvider));
    private readonly IEvaluatableSqlExpressionProvider _evaluatableSqlExpressionProvider = ArgumentGuard.IsNotNull(evaluatableSqlExpressionProvider, nameof(evaluatableSqlExpressionProvider));
    private readonly IPagedDatabaseEntityRetrieverSettingsProvider[] _settingsProviders = ArgumentGuard.IsNotNull(settingsProviders, nameof(settingsProviders)).ToArray();

    public async Task<Result<IPagedDatabaseCommand>> CreatePagedAsync(IEvaluatableContext context, DatabaseOperation operation, int offset, int pageSize, CancellationToken token)
        => await (await new AsyncResultDictionaryBuilder()
            .Add(() => Result.Validate(() => operation == DatabaseOperation.Select, "Only select operation is supported"))
            .Add("Settings", () => GetSettings(context.EntityType))
            .Add("FieldInfo", () => _fieldInfoProvider.Create(context.EntityType).EnsureValue())
            .BuildAsync(token).ConfigureAwait(false))
            .OnSuccessAsync(results => BuildCommandAsync(context, offset, pageSize, results.GetValue<IPagedDatabaseEntityRetrieverSettings>("Settings"), results.GetValue<IEntityFieldInfo>("FieldInfo"), token)).ConfigureAwait(false);

    public Result<IPagedDatabaseEntityRetrieverSettings> Create<TResult>() where TResult : class
        => _settingsProviders
            .Select(x => x.Get<TResult>())
            .WhenNotContinue(() => Result.Invalid<IPagedDatabaseEntityRetrieverSettings>($"No database entity retriever settings provider was found for evaluatable type [{typeof(TResult).FullName}]"));

    private Result<IPagedDatabaseEntityRetrieverSettings> GetSettings(Type entityType)
        => Result.WrapException(() =>
        {
            try
            {
                return ((Result<IPagedDatabaseEntityRetrieverSettings>)GetType()
                    .GetMethod(nameof(Create))
                    .MakeGenericMethod(entityType)
                    .Invoke(this, Array.Empty<object>())).EnsureValue();
            }
            catch (TargetInvocationException ex)
            {
                return Result.Error<IPagedDatabaseEntityRetrieverSettings>(ex.InnerException, "Could not get paged database entity retriever settings, see exception for details");
            }
        });

    private async Task<Result<IPagedDatabaseCommand>> BuildCommandAsync(IEvaluatableContext context, int? offset, int? pageSize, IPagedDatabaseEntityRetrieverSettings settings, IEntityFieldInfo fieldInfo, CancellationToken token)
    {
        string? orderBy = null;
        if (context.OrderByEvaluatable is not null)
        {
            var orderByResult = (await _evaluatableSqlExpressionProvider.GetExpressionAsync(context.OrderByEvaluatable, fieldInfo, null, token).ConfigureAwait(false))
                .OnSuccess(sqlExpression => orderBy = sqlExpression.Expression);
            if (!orderByResult.IsSuccessful())
            {
                return Result.FromExistingResult<IPagedDatabaseCommand>(orderByResult);
            }
        }

        var builder = new PagedSelectCommandBuilder()
            .Select(settings.Fields)
            .From(settings.TableName)
            .Skip(offset)
            .Take(pageSize)
            .OrderBy(orderBy.WhenNullOrEmpty(settings.DefaultOrderBy));

        return (await _evaluatableSqlExpressionProvider.GetExpressionAsync(context.Evaluatable, fieldInfo, null, token).ConfigureAwait(false))
            .OnSuccess(sqlExpression => builder.WithSqlExpression(sqlExpression).Build());
    }
}
