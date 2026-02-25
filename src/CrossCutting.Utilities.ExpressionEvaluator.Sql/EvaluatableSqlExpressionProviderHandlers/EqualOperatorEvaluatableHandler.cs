namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class EqualOperatorEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetConditionExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        if (evaluatable is not EqualOperatorEvaluatable equalOperatorEvaluatable)
        {
            return Result.Continue<string>();
        }

        return (await new AsyncResultDictionaryBuilder()
            .Add(nameof(EqualOperatorEvaluatable.LeftOperand), () => callback.GetConditionExpressionAsync(context, equalOperatorEvaluatable.LeftOperand, fieldNameProvider, parameterBag, callback, token))
            .Add(nameof(EqualOperatorEvaluatable.RightOperand), () => callback.GetConditionExpressionAsync(context, equalOperatorEvaluatable.RightOperand, fieldNameProvider, parameterBag, callback, token))
            .BuildAsync()
            .ConfigureAwait(false))
            .OnSuccess(results => $"{results.GetValue(nameof(EqualOperatorEvaluatable.LeftOperand))} = {results.GetValue(nameof(EqualOperatorEvaluatable.RightOperand))}");
    }
}