namespace CrossCutting.Utilities.Parsers.Expressions;

public class MathematicExpression : IExpression
{
    private readonly IEnumerable<IMathematicExpression> _expressions;

    public MathematicExpression(IEnumerable<IMathematicExpression> expressions)
    {
        ArgumentGuard.IsNotNull(expressions, nameof(expressions));

        _expressions = expressions;
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var state = new MathematicExpressionState(context.Expression, context.Settings.FormatProvider, context.Context, context.Evaluator, (e, _, _) => context.Evaluator.Evaluate(e, context.Settings));
        var error = _expressions
            .Select(x => x.Evaluate(state))
            .FirstOrDefault(x => !x.IsSuccessful());

        if (error is not null)
        {
            return Result.FromExistingResult<object?>(error);
        }

        return state.Results.Count > 0
            ? state.Results.ElementAt(state.Results.Count - 1)
            : Result.Continue<object?>();
    }

    public Result<Type> Validate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var state = new MathematicExpressionState(context.Expression, context.Settings.FormatProvider, context.Context, context.Evaluator, (e, _, _) => context.Evaluator.Evaluate(e, context.Settings));
        var error = _expressions
            .Select(x => x.Evaluate(state))
            .FirstOrDefault(x => !x.IsSuccessful());

        if (error is not null)
        {
            return Result.FromExistingResult<Type>(error);
        }

        return state.Results.Count > 0
            ? Result.FromExistingResult(state.Results.ElementAt(state.Results.Count - 1), state.Results.ElementAt(state.Results.Count - 1).Value?.GetType()!)
            : Result.Continue<Type>();
    }
}
