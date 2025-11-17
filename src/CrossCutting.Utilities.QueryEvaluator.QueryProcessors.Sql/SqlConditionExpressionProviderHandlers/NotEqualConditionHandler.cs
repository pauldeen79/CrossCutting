namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class NotEqualConditionHandler : ConditionExpressionHandlerBase<NotEqualCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(StringBuilder builder, IQueryContext context, NotEqualCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag, CancellationToken token)
        => GetSimpleConditionExpressionAsync(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new ConditionParameters("<>"), token);
}
