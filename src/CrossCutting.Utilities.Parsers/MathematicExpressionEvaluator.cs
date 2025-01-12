namespace CrossCutting.Utilities.Parsers;

public class MathematicExpressionEvaluator : IMathematicExpressionEvaluator
{
    public const string TemporaryDelimiter = "\uE002";

    private readonly IExpressionEvaluator _expressionEvaluator;
    private readonly IEnumerable<IMathematicExpression> _expressions;

    public MathematicExpressionEvaluator(IExpressionEvaluator expressionEvaluator, IEnumerable<IMathematicExpression> expressions)
    {
        ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));
        ArgumentGuard.IsNotNull(expressions, nameof(expressions));

        _expressionEvaluator = expressionEvaluator;
        _expressions = expressions;
    }

    internal static bool IsMathematicExpression(string found)
    {
        ArgumentGuard.IsNotNull(found, nameof(found));

        return Array.Exists(MathematicOperators.Aggregators, x => found.Contains(x.Character.ToString()));
    }

    public Result<object?> Evaluate(string expression, IFormatProvider formatProvider, object? context)
    {
        if (expression is null)
        {
            return Result.Invalid<object?>("Expression is required");
        }

        var state = new MathematicExpressionState(expression, formatProvider, context, Evaluate);
        var error = _expressions
            .Select(x => x.Evaluate(state))
            .FirstOrDefault(x => !x.IsSuccessful());

        if (error is not null)
        {
            return Result.FromExistingResult<object?>(error);
        }

        return state.Results.Count > 0
            ? state.Results.ElementAt(state.Results.Count - 1)
            : _expressionEvaluator
                .Evaluate(expression, formatProvider, context)
                .Transform(x => x.ErrorMessage?.StartsWith("Unknown expression type found in fragment: ") == true
                    ? Result.NotFound<object?>()
                    : x);
    }

    public Result Validate(string expression, IFormatProvider formatProvider, object? context)
    {
        if (expression is null)
        {
            return Result.Invalid("Expression is required");
        }

        var state = new MathematicExpressionState(expression, formatProvider, context, Evaluate);
        var error = _expressions
            .OfType<Validate>()
            .Select(x => x.Evaluate(state))
            .FirstOrDefault(x => !x.IsSuccessful());

        if (error is not null)
        {
            return error;
        }

        return state.Results.Count > 0
            ? state.Results.ElementAt(state.Results.Count - 1)
            : _expressionEvaluator
                .Validate(expression, formatProvider, context)
                .Transform(x => x.ErrorMessage?.StartsWith("Unknown expression type found in fragment: ") == true
                    ? Result.NotFound()
                    : x);
    }
}
