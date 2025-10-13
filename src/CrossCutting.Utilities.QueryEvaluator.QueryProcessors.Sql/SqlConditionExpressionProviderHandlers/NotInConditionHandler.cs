namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class NotInConditionHandler : ConditionExpressionHandlerBase<NotInCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(StringBuilder builder, IQueryContext context, NotInCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetInConditionExpressionAsync(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, "NOT IN");
}
