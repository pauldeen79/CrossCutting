namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Evaluatables;

public partial record SqlLikeEvaluatable
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.NotImplemented<object?>($"{nameof(SqlLikeEvaluatable)} can't be evaluated directly"));
}
