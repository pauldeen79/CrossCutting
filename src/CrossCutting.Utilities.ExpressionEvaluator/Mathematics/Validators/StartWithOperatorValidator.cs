namespace CrossCutting.Utilities.ExpressionEvaluator.Mathematics.Validators;

internal sealed class StartWithOperatorValidator : IMathematicExpressionValidator
{
    public Result<MathematicExpressionState> Validate(MathematicExpressionState state)
    {
        if (state.Context.Expression.TrimStart().StartsWithAny(MathematicOperators.Aggregators.Select(x => x.Character.ToString())))
        {
            return Result.NotFound<MathematicExpressionState>($"Input cannot start with an operator");
        }

        return Result.Success(state);
    }
}
