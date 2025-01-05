namespace CrossCutting.Utilities.Parsers;

public class ExpressionStringEvaluator : IExpressionStringEvaluator
{
    private readonly IFunctionParser _functionParser;
    private readonly IExpressionEvaluator _expressionEvaluator;
    private readonly IFunctionEvaluator _functionEvaluator;
    private readonly IEnumerable<IExpressionString> _expressionStrings;

    public ExpressionStringEvaluator(
        IFunctionParser functionParser,
        IExpressionEvaluator expressionEvaluator,
        IFunctionEvaluator functionEvaluator,
        IEnumerable<IExpressionString> expressionStrings)
    {
        ArgumentGuard.IsNotNull(functionParser, nameof(functionParser));
        ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));
        ArgumentGuard.IsNotNull(functionEvaluator, nameof(functionEvaluator));
        ArgumentGuard.IsNotNull(expressionStrings, nameof(expressionStrings));

        _functionParser = functionParser;
        _expressionEvaluator = expressionEvaluator;
        _functionEvaluator = functionEvaluator;
        _expressionStrings = expressionStrings;
    }

    public Result<object?> Evaluate(string expressionString, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser)
    {
        if (expressionString is null)
        {
            return Result.Invalid<object?>("Expression string is required");
        }

        var state = new ExpressionStringEvaluatorState(expressionString, formatProvider, context, this, formattableStringParser);

        return _expressionStrings
            .OrderBy(x => x.Order)
            .Select(x => x.Evaluate(state))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? EvaluateSimpleExpression(state);
    }

    public Result Validate(string expressionString, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser)
    {
        if (expressionString is null)
        {
            return Result.Invalid("Expression string is required");
        }

        var state = new ExpressionStringEvaluatorState(expressionString, formatProvider, context, this, formattableStringParser);

        return _expressionStrings
            .OrderBy(x => x.Order)
            .Select(x => x.Validate(state))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? ValidateSimpleExpression(state);
    }

    private Result<object?> EvaluateSimpleExpression(ExpressionStringEvaluatorState state)
    {
        // =something else, we can try function
        var functionResult = _functionParser.Parse(state.Input.Substring(1), state.FormatProvider, state.FormattableStringParser, state.Context);
        if (!functionResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(functionResult);
        }

        return _functionEvaluator.Evaluate(functionResult.Value!, _expressionEvaluator, state.FormatProvider, state.Context);
    }

    private Result ValidateSimpleExpression(ExpressionStringEvaluatorState state)
    {
        // =something else, we can try function
        var functionResult = _functionParser.Parse(state.Input.Substring(1), state.FormatProvider, state.FormattableStringParser, state.Context);
        if (!functionResult.IsSuccessful())
        {
            return functionResult;
        }

        return _functionEvaluator.Validate(functionResult.Value!, _expressionEvaluator, state.FormatProvider, state.Context);
    }
}
