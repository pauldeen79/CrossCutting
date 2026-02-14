namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviderHandlers;

public abstract class ConditionExpressionHandlerBase<TCondition> : ISqlConditionExpressionProviderHandler
{
    public async Task<Result> GetConditionExpressionAsync(
        StringBuilder builder,
        IQueryContext context,
        ICondition condition,
        IQueryFieldInfo fieldInfo,
        ISqlExpressionProvider sqlExpressionProvider,
        ParameterBag parameterBag,
        CancellationToken token)
    {
        builder = ArgumentGuard.IsNotNull(builder, nameof(builder));
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        condition = ArgumentGuard.IsNotNull(condition, nameof(condition));
        fieldInfo = ArgumentGuard.IsNotNull(fieldInfo, nameof(fieldInfo));
        sqlExpressionProvider = ArgumentGuard.IsNotNull(sqlExpressionProvider, nameof(sqlExpressionProvider));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        if (condition is not TCondition typedCondition)
        {
            return Result.Continue();
        }

        var expressionHandlerContext = new ConditionExpressionHandlerContext<TCondition>(builder, context, typedCondition, fieldInfo, sqlExpressionProvider, parameterBag);
        return await DoGetConditionExpressionAsync(expressionHandlerContext, token).ConfigureAwait(false);
    }

    protected abstract Task<Result> DoGetConditionExpressionAsync(
        ConditionExpressionHandlerContext<TCondition> context,
        CancellationToken token);

    protected static async Task<Result> GetSimpleConditionExpressionAsync<T>(
        ConditionExpressionHandlerContext<T> context,
        ConditionParameters parameters,
        CancellationToken token) where T : ICompareExpressionContainer
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        parameters = ArgumentGuard.IsNotNull(parameters, nameof(parameters));

        return (await new AsyncResultDictionaryBuilder<string>()
            .Add(nameof(context.Condition.SourceExpression), () => context.SqlExpressionProvider.GetSqlExpressionAsync(context.Context, new SqlExpression(context.Condition.SourceExpression), context.FieldInfo, context.ParameterBag, token))
            .Add(nameof(context.Condition.CompareExpression), () => context.SqlExpressionProvider.GetSqlExpressionAsync(context.Context, new SqlExpression(context.Condition.CompareExpression), context.FieldInfo, context.ParameterBag, token))
            .BuildAsync(token).ConfigureAwait(false))
            .OnSuccess(results => context.Builder.Append($"{results.GetValue(nameof(context.Condition.SourceExpression))} {parameters.Operator} {results.GetValue(nameof(context.Condition.CompareExpression))}"));
    }

    protected static async Task<Result> GetStringConditionExpressionAsync<T>(
        ConditionExpressionHandlerContext<T> context,
        StringConditionParameters parameters,
        CancellationToken token) where T : ICompareExpressionContainer
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        parameters = ArgumentGuard.IsNotNull(parameters, nameof(parameters));

        return (await new AsyncResultDictionaryBuilder<string>()
            .Add(nameof(context.Condition.SourceExpression), () => context.SqlExpressionProvider.GetSqlExpressionAsync(context.Context, new SqlExpression(context.Condition.SourceExpression), context.FieldInfo, context.ParameterBag, token))
            .Add(nameof(context.Condition.CompareExpression), () => context.SqlExpressionProvider.GetSqlExpressionAsync(context.Context, new SqlLikeExpression(context.Condition.CompareExpression, parameters.FormatString), context.FieldInfo, context.ParameterBag, token))
            .BuildAsync(token).ConfigureAwait(false))
            .OnSuccess(results => context.Builder.Append($"{results.GetValue(nameof(context.Condition.SourceExpression))} {parameters.Operator} {results.GetValue(nameof(context.Condition.CompareExpression))}"));
    }

    protected static async Task<Result> GetInConditionExpressionAsync<T>(
        ConditionExpressionHandlerContext<T> context,
        string @operator,
        CancellationToken token) where T : IInCondition
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        @operator = ArgumentGuard.IsNotNullOrEmpty(@operator, nameof(@operator));

        return (await new AsyncResultDictionaryBuilder<string>()
            .Add(nameof(context.Condition.SourceExpression), () => context.SqlExpressionProvider.GetSqlExpressionAsync(context.Context, new SqlExpression(context.Condition.SourceExpression), context.FieldInfo, context.ParameterBag, token))
            .AddRange($"{nameof(context.Condition.CompareExpressions)}.{{0}}", context.Condition.CompareExpressions.Select(x => new Func<Task<Result<string>>>(() => context.SqlExpressionProvider.GetSqlExpressionAsync(context.Context, new SqlExpression(x), context.FieldInfo, context.ParameterBag, token))))
            .BuildAsync(token).ConfigureAwait(false))
            .OnSuccess(results =>
            {
                var secondExpressionValues = results
                    .Where(x => x.Key.StartsWith($"{nameof(context.Condition.CompareExpressions)}."))
                    .Select(x => x.Value.Value);

                context.Builder.Append($"{results.GetValue(nameof(context.Condition.SourceExpression))} {@operator} ({string.Join(", ", secondExpressionValues)})");
            });
    }
}
