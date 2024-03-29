﻿namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFormattableStringParser
{
    Result<string> Parse(string input, IFormatProvider formatProvider, object? context);
}
