namespace CrossCutting.Utilities.Parsers;

public class ExpressionStringParser : IExpressionStringParser
{
    private static readonly IExpressionStringParserProcessor[] _nonSimpleExpressionProcessors = new IExpressionStringParserProcessor[]
    {
        new EmptyExpressionProcessor(),
        new LiteralExpressionProcessor(),
        new OnlyEqualsExpressionProcessor(),
        new FormattableStringExpressionProcessor(),
        new MathematicExpressionProcessor(),
    };

    private readonly IFunctionResultParser _functionResultParser;
    private readonly IExpressionParser _expressionParser;

    public ExpressionStringParser(IFunctionResultParser functionResultParser, IExpressionParser expressionParser)
    {
        _functionResultParser = functionResultParser;
        _expressionParser = expressionParser;
    }

    public Result<object> Parse(
        string input,
        IFormatProvider formatProvider,
        Func<string, Result<string>> placeholderDelegate)
    {
        var state = new ExpressionStringParserState(input, formatProvider, _expressionParser, placeholderDelegate, _functionResultParser);
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
            ? state.FunctionResultParser.Parse(functionResult.Value!)
            : Result<object>.FromExistingResult(functionResult);
    }
}
