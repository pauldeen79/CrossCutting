namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class SmallerOperatorEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        if (evaluatable is not SmallerOperatorEvaluatable smallerOperatorEvaluatable)
        {
            return Result.Continue<string>();
        }

        return (await new AsyncResultDictionaryBuilder()
            .Add(nameof(SmallerOperatorEvaluatable.LeftOperand), () => callback.GetExpressionAsync(context, smallerOperatorEvaluatable.LeftOperand, fieldNameProvider, parameterBag, callback, token))
            .Add(nameof(SmallerOperatorEvaluatable.RightOperand), () => callback.GetExpressionAsync(context, smallerOperatorEvaluatable.RightOperand, fieldNameProvider, parameterBag, callback, token))
            .BuildAsync()
            .ConfigureAwait(false))
            .OnSuccess(results => $"{results.GetValue(nameof(SmallerOperatorEvaluatable.LeftOperand))} < {results.GetValue(nameof(SmallerOperatorEvaluatable.RightOperand))}");
    }
}