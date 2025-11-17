namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class NotNullConditionHandler : ConditionExpressionHandlerBase<NotNullCondition>
{
    protected override async Task<Result> DoGetConditionExpressionAsync(StringBuilder builder, IQueryContext context, NotNullCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag, CancellationToken token)
    {
        sqlExpressionProvider = ArgumentGuard.IsNotNull(sqlExpressionProvider, nameof(sqlExpressionProvider));
        condition = ArgumentGuard.IsNotNull(condition, nameof(condition));

        return (await new AsyncResultDictionaryBuilder<string>()
            .Add(nameof(condition.SourceExpression), () => sqlExpressionProvider.GetSqlExpressionAsync(context, new SqlExpression(condition.SourceExpression), fieldInfo, parameterBag, token))
            .BuildAsync(token).ConfigureAwait(false))
            .OnSuccess(results => builder.Append($"{results.GetValue(nameof(condition.SourceExpression))} IS NOT NULL"));
    }
}
