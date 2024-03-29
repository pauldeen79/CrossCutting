﻿namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

internal static class BaseProcessor
{
    internal static Result<object?> SplitDelimited(ExpressionStringParserState state, char splitDelimiter, Func<string[], Result<object?>> validDelegate)
    {
        if (state.Input.IndexOf(splitDelimiter) == -1)
        {
            return Result.Continue<object?>();
        }

        var split = state.Input.Substring(1).SplitDelimited(splitDelimiter, '\"', true, true);
        if (split.Length == 1)
        {
            return Result.Continue<object?>();
        }

        return validDelegate(split);
    }
}
