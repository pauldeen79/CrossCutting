namespace CrossCutting.Utilities.ExpressionEvaluator;

public sealed class OperatorExpressionToken
{
    public OperatorExpressionTokenType Type { get; }
    public string Value { get; }

    public OperatorExpressionToken(OperatorExpressionTokenType type, string value = "")
    {
        Type = type;
        Value = value;
    }

    public override string ToString() => $"{Type} ({Value})";
}
