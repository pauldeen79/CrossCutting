﻿namespace CrossCutting.Utilities.Parsers.Extensions;

public static class ExpressionParserExtensions
{
    public static Result<object?> Parse(this IExpressionParser instance, string value, IFormatProvider formatProvider)
        => instance.Parse(value, formatProvider, null);
}
