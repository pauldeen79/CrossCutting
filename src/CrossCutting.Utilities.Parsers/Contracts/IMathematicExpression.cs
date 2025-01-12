namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IMathematicExpression
{
    Result<MathematicExpressionState> Evaluate(MathematicExpressionState state);
}
