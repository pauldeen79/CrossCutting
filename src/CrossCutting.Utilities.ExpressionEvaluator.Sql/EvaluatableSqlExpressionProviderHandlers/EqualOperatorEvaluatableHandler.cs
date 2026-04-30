namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class EqualOperatorEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        if (evaluatable is EqualOperatorEvaluatable equalOperatorEvaluatable)
        {
            return await GetExpressionAsync(context, fieldNameProvider, parameterBag, callback, equalOperatorEvaluatable, "=", token)
                .ConfigureAwait(false);
        }

        if (evaluatable is UnaryNegateOperatorEvaluatable unaryNegateOperatorEvaluatable && unaryNegateOperatorEvaluatable.Operand is EqualOperatorEvaluatable innerEqualOperatorEvaluatable)
        {
            return await GetExpressionAsync(context, fieldNameProvider, parameterBag, callback, innerEqualOperatorEvaluatable, "<>", token)
                .ConfigureAwait(false);
        }

        return Result.Continue<string>();
    }

    private static async Task<Result<string>> GetExpressionAsync(object? context, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, EqualOperatorEvaluatable equalOperatorEvaluatable, string @operator, CancellationToken token)
        => (await new AsyncResultDictionaryBuilder()
            .Add(nameof(EqualOperatorEvaluatable.LeftOperand), () => callback.GetExpressionAsync(context, equalOperatorEvaluatable.LeftOperand, fieldNameProvider, parameterBag, callback, token))
            .Add(nameof(EqualOperatorEvaluatable.RightOperand), () => callback.GetExpressionAsync(context, equalOperatorEvaluatable.RightOperand, fieldNameProvider, parameterBag, callback, token))
            .BuildAsync()
            .ConfigureAwait(false))
                .OnSuccess(results => $"{results.GetValue(nameof(EqualOperatorEvaluatable.LeftOperand))} {@operator} {results.GetValue(nameof(EqualOperatorEvaluatable.RightOperand))}");
}