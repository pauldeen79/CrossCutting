﻿namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class StringExpressionParserProcessor : IExpressionParserProcessor
{
    public int Order => 40;

    public Result<object?> Parse(string value, IFormatProvider formatProvider, object? context)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        if (value.StartsWith("\"") && value.EndsWith("\""))
        {
            return Result.Success<object?>(value.Substring(1, value.Length - 2));
        }

        return Result.Continue<object?>();
    }
}
