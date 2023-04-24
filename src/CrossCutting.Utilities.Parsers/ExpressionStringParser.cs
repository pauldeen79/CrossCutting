namespace CrossCutting.Utilities.Parsers;

public class ExpressionStringParser : IExpressionStringParser
{
    private readonly IFunctionResultParser _functionResultParser;
    private readonly IEnumerable<IExpressionStringParserProcessor> _processors;

    public ExpressionStringParser(
        IFunctionResultParser functionResultParser,
        IEnumerable<IExpressionStringParserProcessor> processors)
    {
        _functionResultParser = functionResultParser;
        _processors = processors;
    }

    public Result<object?> Parse(string input, IFormatProvider formatProvider)
    {
        var state = new ExpressionStringParserState(input, formatProvider);
        foreach (var processor in _processors.OrderBy(x => x.Order))
        {
            var result = processor.Process(state);
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        return EvaluateSimpleExpression(state);
    }

    private Result<object?> EvaluateSimpleExpression(ExpressionStringParserState state)
    {
        // =something else, we can try function
        var functionResult = FunctionParser.Parse(state.Input.Substring(1));
        return functionResult.Status == ResultStatus.Ok
            ? _functionResultParser.Parse(functionResult.Value!)
            : Result<object?>.FromExistingResult(functionResult);
    }
}
