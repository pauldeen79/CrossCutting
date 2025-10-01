namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Evaluatables;

public partial record SqlLikeEvaluatable : IEvaluatable
{
    public Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => SourceExpression.EvaluateAsync(context, token);

    IEvaluatableBuilder IBuildableEntity<IEvaluatableBuilder>.ToBuilder() => new SqlLikeEvaluatableBuilder(this);
}
