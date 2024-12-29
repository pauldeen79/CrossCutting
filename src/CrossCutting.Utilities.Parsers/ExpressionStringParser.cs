namespace CrossCutting.Utilities.Parsers;

public class ExpressionStringParser : IExpressionStringParser
{
    private readonly IFunctionParser _functionParser;
    private readonly IExpressionParser _expressionParser;
    private readonly IFunctionEvaluator _evaluator;
    private readonly IEnumerable<IExpressionStringParserProcessor> _processors;

    public ExpressionStringParser(
        IFunctionParser functionParser,
        IExpressionParser expressionParser,
        IFunctionEvaluator evaluator,
        IEnumerable<IExpressionStringParserProcessor> processors)
    {
        ArgumentGuard.IsNotNull(functionParser, nameof(functionParser));
        ArgumentGuard.IsNotNull(expressionParser, nameof(expressionParser));
        ArgumentGuard.IsNotNull(evaluator, nameof(evaluator));
        ArgumentGuard.IsNotNull(processors, nameof(processors));

        _functionParser = functionParser;
        _expressionParser = expressionParser;
        _evaluator = evaluator;
        _processors = processors;
    }

    public Result<object?> Parse(string input, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser)
    {
        if (input is null)
        {
            return Result.Invalid<object?>("Input is required");
        }

        var state = new ExpressionStringParserState(input, formatProvider, context, this, formattableStringParser);

        return _processors
            .OrderBy(x => x.Order)
            .Select(x => x.Process(state))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? EvaluateSimpleExpression(state);
    }

    public Result Validate(string input, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser)
    {
        if (input is null)
        {
            return Result.Invalid("Input is required");
        }

        var state = new ExpressionStringParserState(input, formatProvider, context, this, formattableStringParser);

        return _processors
            .OrderBy(x => x.Order)
            .Select(x => x.Validate(state))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? ValidateSimpleExpression(state);
    }

    private Result<object?> EvaluateSimpleExpression(ExpressionStringParserState state)
    {
        // =something else, we can try function
        var functionResult = _functionParser.Parse(state.Input.Substring(1), state.FormatProvider, state.Context, state.FormattableStringParser);
        if (!functionResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(functionResult);
        }

        return _evaluator.Evaluate(functionResult.Value!, _expressionParser, state.Context);
    }

    private Result ValidateSimpleExpression(ExpressionStringParserState state)
    {
        // =something else, we can try function
        var functionResult = _functionParser.Parse(state.Input.Substring(1), state.FormatProvider, state.Context, state.FormattableStringParser);
        if (!functionResult.IsSuccessful())
        {
            return functionResult;
        }

        return _evaluator.Validate(functionResult.Value!, _expressionParser, state.Context);
    }
}
