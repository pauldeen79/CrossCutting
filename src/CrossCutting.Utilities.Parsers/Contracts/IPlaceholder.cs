﻿namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IPlaceholder
{
    int Order { get; }

    Result Validate(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser);

    Result<GenericFormattableString> Evaluate(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser);
}