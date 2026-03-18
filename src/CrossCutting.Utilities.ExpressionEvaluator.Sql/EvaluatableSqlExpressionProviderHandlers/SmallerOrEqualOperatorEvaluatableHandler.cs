namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class SmallerOrEqualOperatorEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        if (evaluatable is not SmallerOrEqualOperatorEvaluatable smallerorequalOperatorEvaluatable)
        {
            return Result.Continue<string>();
        }

        return (await new AsyncResultDictionaryBuilder()
            .Add(nameof(SmallerOrEqualOperatorEvaluatable.LeftOperand), () => callback.GetExpressionAsync(context, smallerorequalOperatorEvaluatable.LeftOperand, fieldNameProvider, parameterBag, callback, token))
            .Add(nameof(SmallerOrEqualOperatorEvaluatable.RightOperand), () => callback.GetExpressionAsync(context, smallerorequalOperatorEvaluatable.RightOperand, fieldNameProvider, parameterBag, callback, token))
            .BuildAsync()
            .ConfigureAwait(false))
            .OnSuccess(results => $"{results.GetValue(nameof(SmallerOrEqualOperatorEvaluatable.LeftOperand))} <= {results.GetValue(nameof(SmallerOrEqualOperatorEvaluatable.RightOperand))}");
    }
}