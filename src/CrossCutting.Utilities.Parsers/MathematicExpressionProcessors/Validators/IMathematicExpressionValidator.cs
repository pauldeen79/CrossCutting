namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Validators;

internal interface IMathematicExpressionValidator
{
    Result<MathematicExpressionState> Validate(MathematicExpressionState state);
}
