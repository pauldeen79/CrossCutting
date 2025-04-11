namespace CrossCutting.Utilities.ExpressionEvaluator;

internal sealed class OperatorExpressionTokenizerState
{
    public OperatorExpressionTokenizerState(string input)
    {
        Input = input;
    }

    public string Input { get; }
    public List<OperatorExpressionToken> Tokens { get; } = new List<OperatorExpressionToken>();
    public bool InQuotes { get; set; }
    public int Position { get; set; }
}
