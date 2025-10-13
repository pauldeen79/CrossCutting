namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class NullConditionHandler : ConditionExpressionHandlerBase<NullCondition>
{
    protected override async Task<Result> DoGetConditionExpressionAsync(StringBuilder builder, IQueryContext context, NullCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
    {
        sqlExpressionProvider = ArgumentGuard.IsNotNull(sqlExpressionProvider, nameof(sqlExpressionProvider));
        condition = ArgumentGuard.IsNotNull(condition, nameof(condition));

        return (await new AsyncResultDictionaryBuilder<string>()
            .Add(nameof(condition.SourceExpression), sqlExpressionProvider.GetSqlExpressionAsync(context, new SqlExpression(condition.SourceExpression), fieldInfo, parameterBag))
            .Build().ConfigureAwait(false))
            .OnSuccess(results => builder.Append($"{results.GetValue(nameof(condition.SourceExpression))} IS NULL"));
    }
}
