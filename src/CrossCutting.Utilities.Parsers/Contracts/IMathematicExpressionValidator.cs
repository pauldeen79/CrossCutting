namespace CrossCutting.Utilities.Parsers.Contracts;

internal interface IMathematicExpressionValidator
{
    Result<MathematicExpressionState> Validate(MathematicExpressionState state);
}
