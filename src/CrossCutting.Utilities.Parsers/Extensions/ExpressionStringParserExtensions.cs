﻿namespace CrossCutting.Utilities.Parsers.Extensions;

public static class ExpressionStringParserExtensions
{
    public static Result<object?> Parse(this IExpressionStringParser instance, string input, IFormatProvider formatProvider)
        => instance.Parse(input, formatProvider, null, null);

    public static Result<object?> Parse(this IExpressionStringParser instance, string input, IFormatProvider formatProvider, IFormattableStringParser formattableStringParser)
        => instance.Parse(input, formatProvider, null, formattableStringParser);

    public static Result<object?> Parse(this IExpressionStringParser instance, string input, IFormatProvider formatProvider, object? context)
        => instance.Parse(input, formatProvider, context, null);
}
