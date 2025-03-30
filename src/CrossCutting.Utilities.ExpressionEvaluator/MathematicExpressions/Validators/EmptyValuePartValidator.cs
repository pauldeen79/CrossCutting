namespace CrossCutting.Utilities.ExpressionEvaluator.MathematicExpressions.Validators;

internal sealed class EmptyValuePartValidator : IMathematicExpressionValidator
{
    public Result<MathematicExpressionState> Validate(MathematicExpressionState state)
    {
        if (MathematicOperators.Aggregators.Select(x => $"{x.Character}{x.Character}").Any(x => state.Context.Expression.Replace(" ", string.Empty).Contains(x)))
        {
            return Result.NotFound<MathematicExpressionState>($"Input cannot contain operators without values between them");
        }

        return Result.Success(state);
    }
}
