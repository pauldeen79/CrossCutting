namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IMathematicExpressionProcessor
{
    Result<MathematicExpressionState> Process(MathematicExpressionState state);
}
