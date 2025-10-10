namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class NotNullConditionHandler : ConditionExpressionHandlerBase<NotNullCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQueryContext context, NotNullCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => new ResultDictionaryBuilder<string>()
            .Add(nameof(condition.SourceExpression), () => sqlExpressionProvider.GetSqlExpression(context, new SqlExpression(condition.SourceExpression), fieldInfo, parameterBag))
            .Build()
            .OnSuccess(results => builder.Append($"{results.GetValue(nameof(condition.SourceExpression))} IS NOT NULL"));
}
