﻿namespace CrossCutting.Utilities.Parsers;

public static class ExpressionStringParser
{
    private static readonly IExpressionStringParserProcessor[] _nonSimpleExpressionProcessors = new IExpressionStringParserProcessor[]
    {
        new EmptyExpressionProcessor(),
        new LiteralExpressionProcessor(),
        new OnlyEqualsExpressionProcessor(),
        new FormattableStringExpressionProcessor(),
        new MathematicExpressionProcessor(),
    };

    public static Result<object> Parse(
        string input,
        IFormatProvider formatProvider,
        Func<string, IFormatProvider, Result<object>> parseExpressionDelegate,
        Func<string, Result<string>> placeholderDelegate,
        Func<FunctionParseResult, Result<object>> parseFunctionDelegate)
    {
        var state = new ExpressionStringParserState(input, formatProvider, parseExpressionDelegate, placeholderDelegate, parseFunctionDelegate);
        foreach (var processor in _nonSimpleExpressionProcessors)
        {
            var result = processor.Process(state);
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        return EvaluateSimpleExpression(state);
    }

    private static Result<object> EvaluateSimpleExpression(ExpressionStringParserState state)
    {
        // =something else, we can try function
        var functionResult = FunctionParser.Parse(state.Input.Substring(1));
        return functionResult.Status == ResultStatus.Ok
            ? state.ParseFunctionDelegate(functionResult.Value!)
            : Result<object>.FromExistingResult(functionResult);
    }
}