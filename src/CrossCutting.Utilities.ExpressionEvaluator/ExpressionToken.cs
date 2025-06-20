﻿namespace CrossCutting.Utilities.ExpressionEvaluator;

public sealed class ExpressionToken
{
    public ExpressionTokenType Type { get; }
    public string Value { get; }

    public ExpressionToken(ExpressionTokenType type, string value = "")
    {
        Type = type;
        Value = value;
    }
}
