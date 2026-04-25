namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class StringEndsWithOperatorEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        if (evaluatable is not StringEndsWithOperatorEvaluatable stringEndsWithOperatorEvaluatable)
        {
            return Result.Continue<string>();
        }

        return (await new AsyncResultDictionaryBuilder()
            .Add(nameof(StringEndsWithOperatorEvaluatable.LeftOperand), () => callback.GetExpressionAsync(context, stringEndsWithOperatorEvaluatable.LeftOperand, fieldNameProvider, parameterBag, callback, token))
            .Add(nameof(StringEndsWithOperatorEvaluatable.RightOperand), () => callback.GetExpressionAsync(context, stringEndsWithOperatorEvaluatable.RightOperand, fieldNameProvider, parameterBag, callback, token))
            .BuildAsync()
            .ConfigureAwait(false))
            .OnSuccess(results =>
            {
                var rightOperand = results.GetValue(nameof(StringEndsWithOperatorEvaluatable.RightOperand)).ToStringWithNullCheck();
                return parameterBag.ReplaceString(rightOperand, "%{0}")
                    .OnSuccess(() => $"{results.GetValue(nameof(StringEndsWithOperatorEvaluatable.LeftOperand))} LIKE {rightOperand}");
            });
    }
}