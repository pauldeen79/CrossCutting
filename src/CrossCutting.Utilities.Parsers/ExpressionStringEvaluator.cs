namespace CrossCutting.Utilities.Parsers;

public class ExpressionStringEvaluator : IExpressionStringEvaluator
{
    private readonly IFunctionParser _functionParser;
    private readonly IFunctionEvaluator _functionEvaluator;
    private readonly IEnumerable<IExpressionString> _expressionStrings;

    public ExpressionStringEvaluator(
        IFunctionParser functionParser,
        IFunctionEvaluator functionEvaluator,
        IEnumerable<IExpressionString> expressionStrings)
    {
        ArgumentGuard.IsNotNull(functionParser, nameof(functionParser));
        ArgumentGuard.IsNotNull(functionEvaluator, nameof(functionEvaluator));
        ArgumentGuard.IsNotNull(expressionStrings, nameof(expressionStrings));

        _functionParser = functionParser;
        _functionEvaluator = functionEvaluator;
        _expressionStrings = expressionStrings;
    }

    public Result<object?> Evaluate(string expressionString, ExpressionStringEvaluatorSettings settings, object? context, IFormattableStringParser? formattableStringParser)
    {
        if (expressionString is null)
        {
            return Result.Invalid<object?>("Expression string is required");
        }

        var state = new ExpressionStringEvaluatorContext(expressionString, settings, context, this, formattableStringParser);

        return _expressionStrings
            .Select(x => x.Evaluate(state))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? EvaluateSimpleExpression(state);
    }

    public Result<Type> Validate(string expressionString, ExpressionStringEvaluatorSettings settings, object? context, IFormattableStringParser? formattableStringParser)
    {
        if (expressionString is null)
        {
            return Result.Invalid<Type>("Expression string is required");
        }

        var state = new ExpressionStringEvaluatorContext(expressionString, settings, context, this, formattableStringParser);

        return _expressionStrings
            .Select(x => x.Validate(state))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? ValidateSimpleExpression(state);
    }

    private Result<object?> EvaluateSimpleExpression(ExpressionStringEvaluatorContext context)
    {
        // =something else, we can try function
        var functionResult = _functionParser.Parse(context.Input.Substring(1), new FunctionParserSettings(context.Settings.FormatProvider, context.FormattableStringParser), context.Context);
        if (!functionResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(functionResult);
        }

        return _functionEvaluator.Evaluate(functionResult.Value!, new FunctionEvaluatorSettings(context.Settings.FormatProvider, context.Settings.ValidateArgumentTypes), context.Context);
    }

    private Result<Type> ValidateSimpleExpression(ExpressionStringEvaluatorContext context)
    {
        // =something else, we can try function
        var functionResult = _functionParser.Parse(context.Input.Substring(1), new FunctionParserSettings(context.Settings.FormatProvider, context.FormattableStringParser), context.Context);
        if (!functionResult.IsSuccessful())
        {
            return Result.FromExistingResult<Type>(functionResult);
        }

        return _functionEvaluator.Validate(functionResult.Value!, new FunctionEvaluatorSettings(context.Settings.FormatProvider, context.Settings.ValidateArgumentTypes), context.Context);
    }
}
