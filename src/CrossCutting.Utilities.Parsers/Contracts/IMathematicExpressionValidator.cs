namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IMathematicExpressionValidator
{
    Result<MathematicExpressionState> Validate(MathematicExpressionState state);
}
