﻿namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

internal class EmptyExpressionProcessor : IExpressionStringParserProcessor
{
    public Result<object> Process(ExpressionStringParserState state)
    {
        if (string.IsNullOrEmpty(state.Input))
        {
            return Result<object>.Success(string.Empty);
        }

        return Result<object>.Continue();
    }
}