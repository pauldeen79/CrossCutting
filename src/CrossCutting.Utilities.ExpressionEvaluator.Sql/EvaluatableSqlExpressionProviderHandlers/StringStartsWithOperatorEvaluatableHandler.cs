namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class StringStartsWithOperatorEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        if (evaluatable is StringStartsWithOperatorEvaluatable stringStartsWithOperatorEvaluatable)
        {
            return await GetExpressionAsync(context, fieldNameProvider, parameterBag, callback, stringStartsWithOperatorEvaluatable, "LIKE", token)
                .ConfigureAwait(false);
        }

        if (evaluatable is UnaryNegateOperatorEvaluatable unaryNegateOperatorEvaluatable && unaryNegateOperatorEvaluatable.Operand is StringStartsWithOperatorEvaluatable innerStringStartsWithOperatorEvaluatable)
        {
            return await GetExpressionAsync(context, fieldNameProvider, parameterBag, callback, innerStringStartsWithOperatorEvaluatable, "NOT LIKE", token)
                .ConfigureAwait(false);
        }

        return Result.Continue<string>();
    }

    private static async Task<Result<string>> GetExpressionAsync(object? context, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, StringStartsWithOperatorEvaluatable stringStartsWithOperatorEvaluatable, string @operator, CancellationToken token)
        => (await new AsyncResultDictionaryBuilder()
            .Add(nameof(StringStartsWithOperatorEvaluatable.LeftOperand), () => callback.GetExpressionAsync(context, stringStartsWithOperatorEvaluatable.LeftOperand, fieldNameProvider, parameterBag, callback, token))
            .Add(nameof(StringStartsWithOperatorEvaluatable.RightOperand), () => callback.GetExpressionAsync(context, stringStartsWithOperatorEvaluatable.RightOperand, fieldNameProvider, parameterBag, callback, token))
            .BuildAsync()
            .ConfigureAwait(false))
                .OnSuccess(results =>
                {
                    var rightOperand = results.GetValue(nameof(StringStartsWithOperatorEvaluatable.RightOperand)).ToStringWithNullCheck();
                    return parameterBag.ReplaceString(rightOperand, "{0}%")
                        .OnSuccess(() => $"{results.GetValue(nameof(StringStartsWithOperatorEvaluatable.LeftOperand))} {@operator} {rightOperand}");
                });
}