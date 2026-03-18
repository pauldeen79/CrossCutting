namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class NotEqualOperatorEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        if (evaluatable is not NotEqualOperatorEvaluatable notEqualOperatorEvaluatable)
        {
            return Result.Continue<string>();
        }

        return (await new AsyncResultDictionaryBuilder()
            .Add(nameof(NotEqualOperatorEvaluatable.LeftOperand), () => callback.GetExpressionAsync(context, notEqualOperatorEvaluatable.LeftOperand, fieldNameProvider, parameterBag, callback, token))
            .Add(nameof(NotEqualOperatorEvaluatable.RightOperand), () => callback.GetExpressionAsync(context, notEqualOperatorEvaluatable.RightOperand, fieldNameProvider, parameterBag, callback, token))
            .BuildAsync()
            .ConfigureAwait(false))
            .OnSuccess(results => $"{results.GetValue(nameof(NotEqualOperatorEvaluatable.LeftOperand))} <> {results.GetValue(nameof(NotEqualOperatorEvaluatable.RightOperand))}");
    }
}