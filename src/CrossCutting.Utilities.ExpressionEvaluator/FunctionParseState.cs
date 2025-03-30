namespace CrossCutting.Utilities.ExpressionEvaluator;

internal class FunctionParseState
{
    public FunctionParseState(string expression)
    {
        Expression = expression;
    }

    public string Expression { get; }
    public StringBuilder NameBuilder { get; } = new StringBuilder();
    public StringBuilder GenericsBuilder { get; } = new StringBuilder();
    public StringBuilder ArgumentBuilder { get; } = new StringBuilder();
    public List<string> Arguments { get; } = new List<string>();
    public bool NameComplete { get; set; }
    public bool GenericsStarted { get; set; }
    public bool GenericsComplete { get; set; }
    public bool ArgumentsStarted { get; set; }
    public bool ArgumentsComplete { get; set; }
    public bool InQuotes { get; set; }
    public int Index { get; set; }
    public char CurrentCharacter { get; set; }
    public int BracketCount { get; set; }

    public bool IsWhiteSpace() => CurrentCharacter == ' ' || CurrentCharacter == '\r' || CurrentCharacter == '\n' || CurrentCharacter == '\t';
}
