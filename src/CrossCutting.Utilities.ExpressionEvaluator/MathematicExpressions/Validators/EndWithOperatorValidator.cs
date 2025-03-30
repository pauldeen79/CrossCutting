namespace CrossCutting.Utilities.ExpressionEvaluator.MathematicExpressions.Validators;

internal sealed class EndWithOperatorValidator : IMathematicExpressionValidator
{
    public Result<MathematicExpressionState> Validate(MathematicExpressionState state)
    {
        if (state.Context.Expression.TrimEnd().EndsWithAny(MathematicOperators.Aggregators.Select(x => x.Character.ToString())))
        {
            return Result.NotFound<MathematicExpressionState>($"Input cannot end with an operator");
        }

        return Result.Success(state);
    }
}
