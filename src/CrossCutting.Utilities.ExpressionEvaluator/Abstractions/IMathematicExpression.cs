namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IMathematicExpression
{
    Result<MathematicExpressionState> Evaluate(MathematicExpressionState state);
}
