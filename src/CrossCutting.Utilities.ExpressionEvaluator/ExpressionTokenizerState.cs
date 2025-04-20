namespace CrossCutting.Utilities.ExpressionEvaluator;

internal sealed class ExpressionTokenizerState
{
    public ExpressionTokenizerState(string input)
    {
        Input = input;
    }

    public string Input { get; }
    public List<ExpressionToken> Tokens { get; } = new List<ExpressionToken>();
    public bool InQuotes { get; set; }
    public int Position { get; set; }
}
