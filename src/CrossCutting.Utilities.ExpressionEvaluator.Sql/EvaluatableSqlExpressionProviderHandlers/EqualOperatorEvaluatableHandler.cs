namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class EqualOperatorEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        if (evaluatable is EqualOperatorEvaluatable equalOperatorEvaluatable)
        {
            return (await new AsyncResultDictionaryBuilder()
                .Add(nameof(EqualOperatorEvaluatable.LeftOperand), () => callback.GetExpressionAsync(context, equalOperatorEvaluatable.LeftOperand, fieldNameProvider, parameterBag, callback, token))
                .Add(nameof(EqualOperatorEvaluatable.RightOperand), () => callback.GetExpressionAsync(context, equalOperatorEvaluatable.RightOperand, fieldNameProvider, parameterBag, callback, token))
                .BuildAsync()
                .ConfigureAwait(false))
                .OnSuccess(results => $"{results.GetValue(nameof(EqualOperatorEvaluatable.LeftOperand))} = {results.GetValue(nameof(EqualOperatorEvaluatable.RightOperand))}");
        }

        if (evaluatable is UnaryNegateOperatorEvaluatable unaryNegateOperatorEvaluatable && unaryNegateOperatorEvaluatable.Operand is EqualOperatorEvaluatable innerEqualOperatorEvaluatable)
        {
            return (await new AsyncResultDictionaryBuilder()
                .Add(nameof(EqualOperatorEvaluatable.LeftOperand), () => callback.GetExpressionAsync(context, innerEqualOperatorEvaluatable.LeftOperand, fieldNameProvider, parameterBag, callback, token))
                .Add(nameof(EqualOperatorEvaluatable.RightOperand), () => callback.GetExpressionAsync(context, innerEqualOperatorEvaluatable.RightOperand, fieldNameProvider, parameterBag, callback, token))
                .BuildAsync()
                .ConfigureAwait(false))
                .OnSuccess(results => $"{results.GetValue(nameof(EqualOperatorEvaluatable.LeftOperand))} <> {results.GetValue(nameof(EqualOperatorEvaluatable.RightOperand))}");
        }

        return Result.Continue<string>();
    }
}