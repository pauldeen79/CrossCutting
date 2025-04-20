namespace CrossCutting.Utilities.ExpressionEvaluator;

internal sealed class ExpressionParserState
{
    public ExpressionParserState(ICollection<ExpressionToken> tokens)
    {
        Tokens = tokens;
    }

    public ICollection<ExpressionToken> Tokens { get; }
    public int Position { get; set; }
}
