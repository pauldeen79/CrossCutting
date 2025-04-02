namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class MathematicExpression : IExpression
{
    public const string TemporaryDelimiter = "\uE002";

    private readonly IEnumerable<IMathematicExpression> _expressions;

    public MathematicExpression(IEnumerable<IMathematicExpression> expressions)
    {
        ArgumentGuard.IsNotNull(expressions, nameof(expressions));

        _expressions = expressions;
    }

    internal static bool IsMathematicExpression(string found)
    {
        ArgumentGuard.IsNotNull(found, nameof(found));

        return Array.Exists(MathematicOperators.Aggregators, x => found.Contains(x.Character.ToString()));
    }

    public int Order => 50;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        ArgumentGuard.IsNotNull(context, nameof(context));

        var state = new MathematicExpressionState(context, Evaluate);
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

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        ArgumentGuard.IsNotNull(context, nameof(context));

        var state = new MathematicExpressionState(context, Evaluate);
        var error = _expressions
            .Select(x => x.Parse(state))
            .FirstOrDefault(x => !x.IsSuccessful());

        if (error is not null)
        {
            return new ExpressionParseResultBuilder()
                .WithStatus(error.Status)
                .WithErrorMessage(error.ErrorMessage)
                .AddValidationErrors(error.ValidationErrors)
                .WithExpressionType(typeof(MathematicExpression))
                .WithSourceExpression(context.Expression);
        }

        if (state.ParseResults.Count == 0)
        {
            return new ExpressionParseResultBuilder()
                .WithStatus(ResultStatus.Ok)
                .WithExpressionType(typeof(MathematicExpression))
                .WithSourceExpression(context.Expression);
        }

        var result = state.ParseResults.ElementAt(state.ParseResults.Count - 1);

        return new ExpressionParseResultBuilder()
            .WithStatus(ResultStatus.Ok)
            .WithExpressionType(typeof(MathematicExpression))
            .WithSourceExpression(context.Expression)
            .WithResultType(result.ResultType);
    }
}
