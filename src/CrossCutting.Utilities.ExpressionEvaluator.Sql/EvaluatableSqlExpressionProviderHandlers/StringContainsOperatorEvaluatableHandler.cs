namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class StringContainsOperatorEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        if (evaluatable is not StringContainsOperatorEvaluatable stringContainsOperatorEvaluatable)
        {
            return Result.Continue<string>();
        }

        return (await new AsyncResultDictionaryBuilder()
            .Add(nameof(StringContainsOperatorEvaluatable.LeftOperand), () => callback.GetExpressionAsync(context, stringContainsOperatorEvaluatable.LeftOperand, fieldNameProvider, parameterBag, callback, token))
            .Add(nameof(StringContainsOperatorEvaluatable.RightOperand), () => callback.GetExpressionAsync(context, stringContainsOperatorEvaluatable.RightOperand, fieldNameProvider, parameterBag, callback, token))
            .BuildAsync()
            .ConfigureAwait(false))
            .OnSuccess(results =>
            {
                var rightOperand = results.GetValue(nameof(StringContainsOperatorEvaluatable.RightOperand)).ToStringWithNullCheck();
                return parameterBag.ReplaceString(rightOperand, "%{0}%")
                    .OnSuccess(() => $"{results.GetValue(nameof(StringContainsOperatorEvaluatable.LeftOperand))} LIKE {rightOperand}");
            });
    }
}