namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Expressions;

public partial record SqlLikeExpression : IExpression
{
    public Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => SourceExpression.EvaluateAsync(context, token);

    IExpressionBuilder IBuildableEntity<IExpressionBuilder>.ToBuilder() => new SqlLikeExpressionBuilder(this);

    public Task<ExpressionParseResult> ParseAsync(CancellationToken token)
        => Task.FromResult(new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithStatus(ResultStatus.NotSupported).Build());
}
