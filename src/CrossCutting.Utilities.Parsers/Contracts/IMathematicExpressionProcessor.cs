namespace CrossCutting.Utilities.Parsers.Contracts;

internal interface IMathematicExpressionProcessor
{
    Result<MathematicExpressionState> Process(MathematicExpressionState state);
}
