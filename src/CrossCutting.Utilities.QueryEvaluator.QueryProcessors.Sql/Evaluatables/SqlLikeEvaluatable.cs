namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Evaluatables;

public partial record SqlLikeEvaluatable
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => SourceExpression.EvaluateAsync(context, token);
}
