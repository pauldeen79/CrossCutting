namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class InConditionHandler : ConditionExpressionHandlerBase<InCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(StringBuilder builder, IQueryContext context, InCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetInConditionExpressionAsync(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, "IN");
}
