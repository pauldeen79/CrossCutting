namespace CrossCutting.Utilities.Parsers;

public class ExpressionStringParser : IExpressionStringParser
{
    private readonly IEnumerable<IFunctionResultParser> _functionResultParsers;
    private readonly IFunctionParser _functionParser;
    private readonly IEnumerable<IExpressionStringParserProcessor> _processors;

    public ExpressionStringParser(
        IFunctionParser functionParser,
        IEnumerable<IFunctionResultParser> functionResultParsers,
        IEnumerable<IExpressionStringParserProcessor> processors)
    {
        _functionResultParsers = functionResultParsers;
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

        if (!functionResult.IsSuccessful())
        {
            return Result<object?>.FromExistingResult(functionResult);
        }

        return _functionResultParsers
            .Select(x => x.Parse(functionResult.Value!))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result<object?>.NotSupported($"Unknown function found: {functionResult.Value?.FunctionName}");
    }
}
