namespace CrossCutting.Utilities.Parsers;

public class ExpressionStringParser : IExpressionStringParser
{
    private readonly IFunctionParser _functionParser;
    private readonly IExpressionParser _expressionParser;
    private readonly IFunctionParseResultEvaluator _evaluator;
    private readonly IEnumerable<IExpressionStringParserProcessor> _processors;

    public ExpressionStringParser(
        IFunctionParser functionParser,
        IExpressionParser expressionParser,
        IFunctionParseResultEvaluator evaluator,
        IEnumerable<IExpressionStringParserProcessor> processors)
    {
        _functionParser = functionParser;
        _expressionParser = expressionParser;
        _evaluator = evaluator;
        _processors = processors;
    }

    public Result<object?> Parse(string input, IFormatProvider formatProvider, object? context)
    {
        var state = new ExpressionStringParserState(input, formatProvider, context, this);
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

        return _evaluator.Evaluate(functionResult.Value!, _expressionParser, state.Context);
    }
}
