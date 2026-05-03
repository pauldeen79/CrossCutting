namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class GreaterOrEqualOperatorEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        if (evaluatable is GreaterOrEqualOperatorEvaluatable greaterOrEqualOperatorEvaluatable)
        {
            return await GetExpressionAsync(context, fieldNameProvider, parameterBag, callback, greaterOrEqualOperatorEvaluatable, ">=", token)
                .ConfigureAwait(false);
        }

        if (evaluatable is UnaryNegateOperatorEvaluatable unaryNegateOperatorEvaluatable && unaryNegateOperatorEvaluatable.Operand is GreaterOrEqualOperatorEvaluatable innerGreaterOrEqualOperatorEvaluatable)
        {
            return await GetExpressionAsync(context, fieldNameProvider, parameterBag, callback, innerGreaterOrEqualOperatorEvaluatable, "<", token)
                .ConfigureAwait(false);
        }

        return Result.Continue<string>();
    }

    private static async Task<Result<string>> GetExpressionAsync(object? context, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, GreaterOrEqualOperatorEvaluatable greaterOrEqualOperatorEvaluatable, string @operator, CancellationToken token)
    {
        return (await new AsyncResultDictionaryBuilder()
            .Add(nameof(GreaterOrEqualOperatorEvaluatable.LeftOperand), () => callback.GetExpressionAsync(context, greaterOrEqualOperatorEvaluatable.LeftOperand, fieldNameProvider, parameterBag, callback, token))
            .Add(nameof(GreaterOrEqualOperatorEvaluatable.RightOperand), () => callback.GetExpressionAsync(context, greaterOrEqualOperatorEvaluatable.RightOperand, fieldNameProvider, parameterBag, callback, token))
            .BuildAsync()
            .ConfigureAwait(false))
                .OnSuccess(results => $"{results.GetValue(nameof(GreaterOrEqualOperatorEvaluatable.LeftOperand))} {@operator} {results.GetValue(nameof(GreaterOrEqualOperatorEvaluatable.RightOperand))}");
    }
}