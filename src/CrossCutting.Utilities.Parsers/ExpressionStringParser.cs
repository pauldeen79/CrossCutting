namespace CrossCutting.Utilities.Parsers;

public class ExpressionStringParser
{
    private static readonly IExpressionStringParserProcessor[] _nonSimpleExpressionProcessors = new IExpressionStringParserProcessor[]
    {
        new EmptyExpressionProcessor(),
        new LiteralExpressionProcessor(),
        new OnlyEqualsExpressionProcessor(),
        new FormattableStringExpressionProcessor(),
        new MathematicExpressionProcessor(),
    };

    private readonly IFunctionParser _functionParser;

    public ExpressionStringParser(IFunctionParser functionParser)
    {
        _functionParser = functionParser;
    }

    public Result<object> Parse(
        string input,
        IFormatProvider formatProvider,
        Func<string, IFormatProvider, Result<object>> parseExpressionDelegate,
        Func<string, Result<string>> placeholderDelegate)
    {
        var state = new ExpressionStringParserState(input, formatProvider, parseExpressionDelegate, placeholderDelegate, _functionParser);
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
            ? state.FunctionParser.Parse(functionResult.Value!)
            : Result<object>.FromExistingResult(functionResult);
    }
}
