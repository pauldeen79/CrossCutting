namespace CrossCutting.Utilities.Parsers;

internal interface IMathematicExpressionProcessor
{
    Result<MathematicExpressionState> Process(MathematicExpressionState state);
}
