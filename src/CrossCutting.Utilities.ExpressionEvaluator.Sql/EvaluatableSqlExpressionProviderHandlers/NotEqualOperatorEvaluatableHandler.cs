namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class NotEqualOperatorEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        if (evaluatable is NotEqualOperatorEvaluatable notEqualOperatorEvaluatable)
        {
            return (await new AsyncResultDictionaryBuilder()
                .Add(nameof(NotEqualOperatorEvaluatable.LeftOperand), () => callback.GetExpressionAsync(context, notEqualOperatorEvaluatable.LeftOperand, fieldNameProvider, parameterBag, callback, token))
                .Add(nameof(NotEqualOperatorEvaluatable.RightOperand), () => callback.GetExpressionAsync(context, notEqualOperatorEvaluatable.RightOperand, fieldNameProvider, parameterBag, callback, token))
                .BuildAsync()
                .ConfigureAwait(false))
                .OnSuccess(results => $"{results.GetValue(nameof(NotEqualOperatorEvaluatable.LeftOperand))} <> {results.GetValue(nameof(NotEqualOperatorEvaluatable.RightOperand))}");
        }

        if (evaluatable is UnaryNegateOperatorEvaluatable unaryNegateOperatorEvaluatable && unaryNegateOperatorEvaluatable.Operand is NotEqualOperatorEvaluatable innerEqualOperatorEvaluatable)
        {
            return (await new AsyncResultDictionaryBuilder()
                .Add(nameof(EqualOperatorEvaluatable.LeftOperand), () => callback.GetExpressionAsync(context, innerEqualOperatorEvaluatable.LeftOperand, fieldNameProvider, parameterBag, callback, token))
                .Add(nameof(EqualOperatorEvaluatable.RightOperand), () => callback.GetExpressionAsync(context, innerEqualOperatorEvaluatable.RightOperand, fieldNameProvider, parameterBag, callback, token))
                .BuildAsync()
                .ConfigureAwait(false))
                .OnSuccess(results => $"{results.GetValue(nameof(NotEqualOperatorEvaluatable.LeftOperand))} = {results.GetValue(nameof(NotEqualOperatorEvaluatable.RightOperand))}");
        }

        return Result.Continue<string>();
    }
}