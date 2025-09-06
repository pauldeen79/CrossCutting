namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class NullConditionHandler : ConditionExpressionHandlerBase<NullCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, object? context, NullCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => new ResultDictionaryBuilder<string>()
            .Add(nameof(condition.SourceExpression), () => sqlExpressionProvider.GetSqlExpression(query, context, condition.SourceExpression, fieldInfo, parameterBag))
            .Build()
            .OnSuccess(results => builder.Append($"{results.GetValue(nameof(condition.SourceExpression))} IS NULL"));
}
