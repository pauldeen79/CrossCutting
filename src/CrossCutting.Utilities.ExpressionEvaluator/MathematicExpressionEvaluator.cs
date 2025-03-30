namespace CrossCutting.Utilities.ExpressionEvaluator;

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

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        ArgumentGuard.IsNotNull(context, nameof(context));

        var state = new MathematicExpressionState(context, _expressionEvaluator, Evaluate);
        var error = _expressions
            .Select(x => x.Evaluate(state))
            .FirstOrDefault(x => !x.IsSuccessful());

        if (error is not null)
        {
            return Result.FromExistingResult<object?>(error);
        }

        return state.Results.Count > 0
            ? state.Results.ElementAt(state.Results.Count - 1)
            : context.Evaluate(context.Expression)
                .Transform(x => x.ErrorMessage?.StartsWith("Unknown expression type found in fragment: ") == true
                    ? Result.NotFound<object?>()
                    : x);
    }

    public Result<Type> Validate(ExpressionEvaluatorContext context)
    {
        ArgumentGuard.IsNotNull(context, nameof(context));

        var state = new MathematicExpressionState(context, _expressionEvaluator, Evaluate);
        var error = _expressions
            .Select(x => x.Evaluate(state))
            .FirstOrDefault(x => !x.IsSuccessful());

        if (error is not null)
        {
            return Result.FromExistingResult<Type>(error);
        }

        return state.Results.Count > 0
            ? Result.FromExistingResult(state.Results.ElementAt(state.Results.Count - 1), state.Results.ElementAt(state.Results.Count - 1).Value?.GetType()!)
            : context.Validate<Type>()
                .Transform(x => x.ErrorMessage?.StartsWith("Unknown expression type found in fragment: ") == true
                    ? Result.NotFound<Type>()
                    : x);
    }
}
