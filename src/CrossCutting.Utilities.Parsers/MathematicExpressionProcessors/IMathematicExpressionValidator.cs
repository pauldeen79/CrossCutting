namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors;

internal interface IMathematicExpressionValidator
{
    Result<MathematicExpressionState> Validate(MathematicExpressionState state);
}
