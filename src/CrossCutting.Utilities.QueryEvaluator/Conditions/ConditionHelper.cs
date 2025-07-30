namespace CrossCutting.Utilities.QueryEvaluator.Core.Conditions;

public static class ConditionHelper
{
    public static Task<Result<bool>> EvaluateStringConditionAsync(
        Abstractions.IExpression firstExpression,
        Abstractions.IExpression secondExpression,
        ExpressionEvaluatorContext context,
        Func<string, string, bool> resultDelegate,
        CancellationToken token)
        => EvaluateObjectConditionAsync(firstExpression, secondExpression, context,
            (first, second) => first is string firstString
                    && second is string secondString
                        ? Result.Success(resultDelegate(firstString, secondString))
                        : Result.Invalid<bool>("LeftValue and RightValue both need to be of type string"), token);

    public static async Task<Result<bool>> EvaluateObjectConditionAsync(
        Abstractions.IExpression firstExpression,
        ExpressionEvaluatorContext context,
        Func<object?, bool> resultDelegate,
        CancellationToken token)
    {
        firstExpression = ArgumentGuard.IsNotNull(firstExpression, nameof(firstExpression));
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        resultDelegate = ArgumentGuard.IsNotNull(resultDelegate, nameof(resultDelegate));

        return (await new AsyncResultDictionaryBuilder()
            .Add(nameof(firstExpression), firstExpression.EvaluateAsync(context, token))
            .Build()
            .ConfigureAwait(false))
            .OnSuccess(results => resultDelegate(results.GetValue(nameof(firstExpression))));
    }

    public static async Task<Result<bool>> EvaluateObjectConditionAsync(
        Abstractions.IExpression firstExpression,
        Abstractions.IExpression secondExpression,
        ExpressionEvaluatorContext context,
        Func<object?, object?, Result<bool>> resultDelegate,
        CancellationToken token)
    {
        firstExpression = ArgumentGuard.IsNotNull(firstExpression, nameof(firstExpression));
        secondExpression = ArgumentGuard.IsNotNull(secondExpression, nameof(firstExpression));
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        resultDelegate = ArgumentGuard.IsNotNull(resultDelegate, nameof(resultDelegate));

        return (await new AsyncResultDictionaryBuilder()
            .Add(nameof(firstExpression), firstExpression.EvaluateAsync(context, token))
            .Add(nameof(secondExpression), secondExpression.EvaluateAsync(context, token))
            .Build()
            .ConfigureAwait(false))
            .OnSuccess(results => resultDelegate(results.GetValue(nameof(firstExpression)), results.GetValue(nameof(secondExpression))));
    }
}
