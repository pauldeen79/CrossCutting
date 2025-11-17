namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class SmallerThanOrEqualConditionHandler : ConditionExpressionHandlerBase<SmallerThanOrEqualCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(StringBuilder builder, IQueryContext context, SmallerThanOrEqualCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag, CancellationToken token)
        => GetSimpleConditionExpressionAsync(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new ConditionParameters("<="), token);
}
