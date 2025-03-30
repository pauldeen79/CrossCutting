namespace CrossCutting.Utilities.ExpressionEvaluator.MathematicExpressions.Validators;

internal sealed class NullOrEmptyValidator : IMathematicExpressionValidator
{
    public Result<MathematicExpressionState> Validate(MathematicExpressionState state)
    {
        if (string.IsNullOrEmpty(state.Context.Expression))
        {
            return Result.Invalid<MathematicExpressionState>("Input cannot be null or empty");
        }

        return Result.Success(state);
    }
}
