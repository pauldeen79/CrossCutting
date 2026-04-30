namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class SmallerOrEqualOperatorEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        if (evaluatable is SmallerOrEqualOperatorEvaluatable smallerOrEqualOperatorEvaluatable)
        {
            return await GetExpressionAsync(context, fieldNameProvider, parameterBag, callback, smallerOrEqualOperatorEvaluatable, "<=", token)
                .ConfigureAwait(false);
        }

        if (evaluatable is UnaryNegateOperatorEvaluatable unaryNegateOperatorEvaluatable && unaryNegateOperatorEvaluatable.Operand is SmallerOrEqualOperatorEvaluatable innerSmallerOrEqualOperatorEvaluatable)
        {
            return await GetExpressionAsync(context, fieldNameProvider, parameterBag, callback, innerSmallerOrEqualOperatorEvaluatable, ">", token)
                .ConfigureAwait(false);
        }

        return Result.Continue<string>();
    }

    private static async Task<Result<string>> GetExpressionAsync(object? context, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, SmallerOrEqualOperatorEvaluatable smallerOrEqualOperatorEvaluatable, string @operator, CancellationToken token)
        => (await new AsyncResultDictionaryBuilder()
            .Add(nameof(SmallerOrEqualOperatorEvaluatable.LeftOperand), () => callback.GetExpressionAsync(context, smallerOrEqualOperatorEvaluatable.LeftOperand, fieldNameProvider, parameterBag, callback, token))
            .Add(nameof(SmallerOrEqualOperatorEvaluatable.RightOperand), () => callback.GetExpressionAsync(context, smallerOrEqualOperatorEvaluatable.RightOperand, fieldNameProvider, parameterBag, callback, token))
            .BuildAsync()
            .ConfigureAwait(false))
                .OnSuccess(results => $"{results.GetValue(nameof(SmallerOrEqualOperatorEvaluatable.LeftOperand))} {@operator} {results.GetValue(nameof(SmallerOrEqualOperatorEvaluatable.RightOperand))}");
}