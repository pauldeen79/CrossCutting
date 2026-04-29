namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class EmptyEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        if (evaluatable is IEmptyEvaluatable)
        {
            return string.Empty;
        }

        return Result.Continue<string>();
    }
}