namespace CrossCutting.Utilities.ExpressionEvaluator;

internal sealed class OperatorExpressionParserState
{
    public OperatorExpressionParserState(ICollection<OperatorExpressionToken> tokens)
    {
        Tokens = tokens;
    }

    public ICollection<OperatorExpressionToken> Tokens { get; }
    public int Position { get; set; }
}
