namespace CrossCutting.Utilities.Parsers;

public class ExpressionStringParser : IExpressionStringParser
{
    private readonly IFunctionResultParser _functionResultParser;
    private readonly IFunctionParser _functionParser;
    private readonly IEnumerable<IExpressionStringParserProcessor> _processors;

    public ExpressionStringParser(
        IFunctionResultParser functionResultParser,
        IFunctionParser functionParser,
        IEnumerable<IExpressionStringParserProcessor> processors)
    {
        _functionResultParser = functionResultParser;
        _functionParser = functionParser;
        _processors = processors;
    }

    public Result<object?> Parse(string input, IFormatProvider formatProvider, object? context)
    {
        var state = new ExpressionStringParserState(input, formatProvider, context);
        return _processors
            .OrderBy(x => x.Order)
            .Select(x => x.Process(state))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? EvaluateSimpleExpression(state);
    }

    private Result<object?> EvaluateSimpleExpression(ExpressionStringParserState state)
    {
        // =something else, we can try function
        var functionResult = _functionParser.Parse(state.Input.Substring(1), state.FormatProvider, state.Context);
        if (functionResult.Status == ResultStatus.NotFound)
        {
            return Result<object?>.FromExistingResult(functionResult);
        }

        return functionResult.Status == ResultStatus.Ok
            ? _functionResultParser.Parse(functionResult.Value!)
            : Result<object?>.FromExistingResult(functionResult);
    }
}
