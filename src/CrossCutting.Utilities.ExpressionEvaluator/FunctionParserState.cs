namespace CrossCutting.Utilities.ExpressionEvaluator;

internal sealed class FunctionParserState
{
    public FunctionParserState(string expression)
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

    public void CompleteName()
    {
        NameComplete = true;
        ArgumentsStarted = CurrentCharacter == '(';
        GenericsStarted = CurrentCharacter == '<';
        BracketCount = CurrentCharacter == '(' ? 1 : 0;
    }
    public void OpenBracket()
    {
        BracketCount++;
        ArgumentBuilder.Append(CurrentCharacter);
    }

    public void CloseBracket()
    {
        BracketCount--;
        if (BracketCount == 0)
        {
            var arg = ArgumentBuilder.ToString().Trim();
            if (!string.IsNullOrEmpty(arg))
            {
                Arguments.Add(arg);
            }
            
            ArgumentsComplete = true;
        }
        else
        {
            ArgumentBuilder.Append(CurrentCharacter);
        }
    }
}
