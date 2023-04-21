namespace CrossCutting.Utilities.Parsers;

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
        Func<string, Result<object>> parseExpressionDelegate,
        Func<string, Result<string>> placeholderDelegate,
        Func<FunctionParseResult, Result<object>> parseFunctionDelegate)
    {
        var state = new ExpressionStringParserState(input, parseExpressionDelegate, placeholderDelegate, parseFunctionDelegate);
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
        return functionResult.Status switch
        {
            ResultStatus.Ok => state.ParseFunctionDelegate(functionResult.Value!),
            ResultStatus.NotSupported => Result<object>.FromExistingResult(functionResult),
            _ => Result<object>.Success(state.Input)
        };
    }
}
