namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class GreaterOrEqualOperatorEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        if (evaluatable is not GreaterOrEqualOperatorEvaluatable greaterorequalOperatorEvaluatable)
        {
            return Result.Continue<string>();
        }

        return (await new AsyncResultDictionaryBuilder()
            .Add(nameof(GreaterOrEqualOperatorEvaluatable.LeftOperand), () => callback.GetExpressionAsync(context, greaterorequalOperatorEvaluatable.LeftOperand, fieldNameProvider, parameterBag, callback, token))
            .Add(nameof(GreaterOrEqualOperatorEvaluatable.RightOperand), () => callback.GetExpressionAsync(context, greaterorequalOperatorEvaluatable.RightOperand, fieldNameProvider, parameterBag, callback, token))
            .BuildAsync()
            .ConfigureAwait(false))
            .OnSuccess(results => $"{results.GetValue(nameof(GreaterOrEqualOperatorEvaluatable.LeftOperand))} >= {results.GetValue(nameof(GreaterOrEqualOperatorEvaluatable.RightOperand))}");
    }
}