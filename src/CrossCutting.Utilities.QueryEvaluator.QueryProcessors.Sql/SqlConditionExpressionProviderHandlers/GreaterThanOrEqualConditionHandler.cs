namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class GreaterThanOrEqualConditionHandler : ConditionExpressionHandlerBase<GreaterThanOrEqualCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(StringBuilder builder, IQueryContext context, GreaterThanOrEqualCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag, CancellationToken token)
        => GetSimpleConditionExpressionAsync(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new ConditionParameters(">="), token);
}
