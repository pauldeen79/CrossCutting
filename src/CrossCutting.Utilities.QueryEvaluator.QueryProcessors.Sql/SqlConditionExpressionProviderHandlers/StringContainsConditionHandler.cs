namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringContainsConditionHandler : ConditionExpressionHandlerBase<StringContainsCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, StringContainsCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => new ResultDictionaryBuilder<string>()
            .Add(nameof(condition.SourceExpression), () => sqlExpressionProvider.GetSqlExpression(query, condition.SourceExpression, fieldInfo, parameterBag))
            .Add(nameof(condition.CompareExpression), () => sqlExpressionProvider.GetSqlExpression(query, new SqlLikeExpression(condition.CompareExpression, "%{0}%"), fieldInfo, parameterBag))
            .Build()
            .OnSuccess(results => builder.Append($"{results.GetValue(nameof(condition.SourceExpression))} LIKE {results.GetValue(nameof(condition.CompareExpression))}"));
}
