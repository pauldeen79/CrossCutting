namespace CrossCutting.Utilities.QueryEvaluator.Conditions;

public partial record StringNotContainsCondition
{
    public async override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await EvaluateTypedAsync(context, token).ConfigureAwait(false);

    public override async Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await new AsyncResultDictionaryBuilder()
            .Add(nameof(FirstExpression), FirstExpression.EvaluateAsync(context, token))
            .Add(nameof(SecondExpression), SecondExpression.EvaluateAsync(context, token))
            .Build()
            .ConfigureAwait(false))
            .OnSuccess(results => results.GetValue(nameof(FirstExpression)) is string firstString
                && results.GetValue(nameof(SecondExpression)) is string secondString
                    ? Result.Success(firstString.IndexOf(secondString, StringComparison) == -1)
                    : Result.Invalid<bool>("LeftValue and RightValue both need to be of type string"));
}
