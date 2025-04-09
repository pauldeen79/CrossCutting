namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionTokenizer;

internal sealed class ExpressionToken
{
    public ExpressionTokenType Type { get; }
    public string Value { get; }

    public ExpressionToken(ExpressionTokenType type, string value = "")
    {
        Type = type;
        Value = value;
    }

    public override string ToString() => $"{Type} ({Value})";
}
