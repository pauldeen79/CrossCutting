namespace CrossCutting.Utilities.Parsers;

internal class MathematicExpressionState
{
    public string Input { get; }
    public string Remainder { get; set; }
    public List<Result<object>> Results { get; } = new();
    public Func<string, Result<object>> ParseExpressionDelegate { get; }
    public Func<string, Func<string, Result<object>>, Result<object>> ParseDelegate { get; }

    public MathematicExpressionState(string input, Func<string, Result<object>> parseExpressionDelegate, Func<string, Func<string, Result<object>>, Result<object>> parseDelegate)
    {
        Input = input;
        Remainder = input;
        ParseExpressionDelegate = parseExpressionDelegate;
        ParseDelegate = parseDelegate;
    }
}
