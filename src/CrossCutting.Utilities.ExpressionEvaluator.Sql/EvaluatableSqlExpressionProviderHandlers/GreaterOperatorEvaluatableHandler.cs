namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class GreaterOperatorEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        if (evaluatable is GreaterOperatorEvaluatable greaterOperatorEvaluatable)
        {
            return await GetExpressionAsync(context, fieldNameProvider, parameterBag, callback, greaterOperatorEvaluatable, ">", token)
                .ConfigureAwait(false);
        }

        if (evaluatable is UnaryNegateOperatorEvaluatable unaryNegateOperatorEvaluatable && unaryNegateOperatorEvaluatable.Operand is GreaterOperatorEvaluatable innerGreaterOperatorEvaluatable)
        {
            return await GetExpressionAsync(context, fieldNameProvider, parameterBag, callback, innerGreaterOperatorEvaluatable, "<=", token)
                .ConfigureAwait(false);
        }

        return Result.Continue<string>();
    }

    private static async Task<Result<string>> GetExpressionAsync(object? context, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, GreaterOperatorEvaluatable greaterOperatorEvaluatable, string @operator, CancellationToken token)
        => (await new AsyncResultDictionaryBuilder()
            .Add(nameof(GreaterOperatorEvaluatable.LeftOperand), () => callback.GetExpressionAsync(context, greaterOperatorEvaluatable.LeftOperand, fieldNameProvider, parameterBag, callback, token))
            .Add(nameof(GreaterOperatorEvaluatable.RightOperand), () => callback.GetExpressionAsync(context, greaterOperatorEvaluatable.RightOperand, fieldNameProvider, parameterBag, callback, token))
            .BuildAsync()
            .ConfigureAwait(false))
                .OnSuccess(results => $"{results.GetValue(nameof(GreaterOperatorEvaluatable.LeftOperand))} {@operator} {results.GetValue(nameof(GreaterOperatorEvaluatable.RightOperand))}");
}