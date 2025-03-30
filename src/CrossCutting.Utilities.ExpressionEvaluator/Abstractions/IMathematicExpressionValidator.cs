namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IMathematicExpressionValidator
{
    Result<MathematicExpressionState> Validate(MathematicExpressionState state);
}
