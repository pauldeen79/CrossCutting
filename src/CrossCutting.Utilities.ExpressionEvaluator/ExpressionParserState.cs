namespace CrossCutting.Utilities.ExpressionEvaluator;

internal sealed class ExpressionParserState
{
    public ExpressionParserState(ICollection<OperatorExpressionToken> tokens)
    {
        Tokens = tokens;
    }

    public ICollection<OperatorExpressionToken> Tokens { get; }
    public int Position { get; set; }
}
