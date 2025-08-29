namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class EqualConditionHandler : ConditionExpressionHandlerBase<EqualCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, EqualCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => new ResultDictionaryBuilder<string>()
            .Add(nameof(condition.FirstExpression), () => sqlExpressionProvider.GetSqlExpression(query, condition.FirstExpression, fieldInfo, parameterBag))
            .Add(nameof(condition.SecondExpression), () => sqlExpressionProvider.GetSqlExpression(query, condition.SecondExpression, fieldInfo, parameterBag))
            .Build()
            .OnSuccess(results => builder.Append($"{results.GetValue(nameof(condition.FirstExpression))} = {results.GetValue(nameof(condition.SecondExpression))}"));
}
