namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class NotInConditionHandler : ConditionExpressionHandlerBase<NotInCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, NotInCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => new ResultDictionaryBuilder<string>()
            .Add(nameof(condition.SourceExpression), () => sqlExpressionProvider.GetSqlExpression(query, condition.SourceExpression, fieldInfo, parameterBag))
            .AddRange($"{nameof(condition.CompareExpressions)}.{{0}}", () => condition.CompareExpressions.Select(x => sqlExpressionProvider.GetSqlExpression(query, x, fieldInfo, parameterBag)))
            .Build()
            .OnSuccess(results =>
            {
                var secondExpressionValues = results
                    .Where(x => x.Key.StartsWith($"{nameof(condition.CompareExpressions)}."))
                    .Select(x => x.Value.Value);

                builder.Append($"{results.GetValue(nameof(condition.SourceExpression))} NOT IN ({string.Join(", ", secondExpressionValues)})");
            });
}
