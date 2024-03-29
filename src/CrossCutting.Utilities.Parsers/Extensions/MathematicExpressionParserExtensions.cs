﻿namespace CrossCutting.Utilities.Parsers.Extensions;

public static class MathematicExpressionParserExtensions
{
    public static Result<object?> Parse(this IMathematicExpressionParser instance, string input, IFormatProvider formatProvider)
        => instance.Parse(input, formatProvider, null);
}
