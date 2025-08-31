namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringEqualsConditionHandler : ConditionExpressionHandlerBase<StringEqualsCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, StringEqualsCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => new ResultDictionaryBuilder<string>()
            .Add(nameof(condition.SourceExpression), () => sqlExpressionProvider.GetSqlExpression(query, condition.SourceExpression, fieldInfo, parameterBag))
            .Add(nameof(condition.CompareExpression), () => sqlExpressionProvider.GetSqlExpression(query, condition.CompareExpression, fieldInfo, parameterBag))
            .Build()
            .OnSuccess(results => builder.Append($"{results.GetValue(nameof(condition.SourceExpression))} = {results.GetValue(nameof(condition.CompareExpression))}"));
}
