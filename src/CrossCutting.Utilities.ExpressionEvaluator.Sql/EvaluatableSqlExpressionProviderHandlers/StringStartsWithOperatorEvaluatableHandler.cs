namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public class StringStartsWithOperatorEvaluatableHandler : IEvaluatableSqlExpressionProviderHandler
{
    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        if (evaluatable is not StringStartsWithOperatorEvaluatable stringStartsWithOperatorEvaluatable)
        {
            return Result.Continue<string>();
        }

        return (await new AsyncResultDictionaryBuilder()
            .Add(nameof(StringStartsWithOperatorEvaluatable.LeftOperand), () => callback.GetExpressionAsync(context, stringStartsWithOperatorEvaluatable.LeftOperand, fieldNameProvider, parameterBag, callback, token))
            .Add(nameof(StringStartsWithOperatorEvaluatable.RightOperand), () => callback.GetExpressionAsync(context, stringStartsWithOperatorEvaluatable.RightOperand, fieldNameProvider, parameterBag, callback, token))
            .BuildAsync()
            .ConfigureAwait(false))
            .OnSuccess(results =>
            {
                var rightOperand = results.GetValue(nameof(StringStartsWithOperatorEvaluatable.RightOperand)).ToStringWithNullCheck();
                return parameterBag.ReplaceString(rightOperand, "{0}%")
                    .OnSuccess(() => $"{results.GetValue(nameof(StringStartsWithOperatorEvaluatable.LeftOperand))} LIKE {rightOperand}");
            });
    }
}