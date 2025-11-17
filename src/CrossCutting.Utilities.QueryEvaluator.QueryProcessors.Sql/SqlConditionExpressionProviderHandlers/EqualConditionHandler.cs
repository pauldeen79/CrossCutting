namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class EqualConditionHandler : ConditionExpressionHandlerBase<EqualCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(StringBuilder builder, IQueryContext context, EqualCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag, CancellationToken token)
        => GetSimpleConditionExpressionAsync(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new ConditionParameters("="), token);
}
