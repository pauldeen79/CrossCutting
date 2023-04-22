namespace CrossCutting.Utilities.Parsers;

public class ExpressionStringParser : IExpressionStringParser
{
    private readonly IFunctionResultParser _functionResultParser;
    private readonly IPlaceholderProcessor _placeholderProcessor;
    private readonly IEnumerable<IExpressionStringParserProcessor> _processors;

    public ExpressionStringParser(
        IFunctionResultParser functionResultParser,
        IPlaceholderProcessor placeholderProcessor,
        IEnumerable<IExpressionStringParserProcessor> processors)
    {
        _functionResultParser = functionResultParser;
        _placeholderProcessor = placeholderProcessor;
        _processors = processors;
    }

    public Result<object> Parse(string input, IFormatProvider formatProvider)
    {
        var state = new ExpressionStringParserState(input, formatProvider, _placeholderProcessor, _functionResultParser);
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

    private static Result<object> EvaluateSimpleExpression(ExpressionStringParserState state)
    {
        // =something else, we can try function
        var functionResult = FunctionParser.Parse(state.Input.Substring(1));
        return functionResult.Status == ResultStatus.Ok
            ? state.FunctionResultParser.Parse(functionResult.Value!)
            : Result<object>.FromExistingResult(functionResult);
    }
}
