namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class TypeOfExpression : IExpression
{
    private static readonly Regex _typeOfRegEx = new(@"^typeof\((?<typename>.+?)\)$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));

    public int Order => 13;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var match = _typeOfRegEx.Match(context.Expression);
        if (!match.Success)
        {
            return Result.Continue<object?>();
        }

        var typename = match.Groups["typename"].Value;

        var type = Type.GetType(typename, false);
        if (type is null)
        {
            return Result.Invalid<object?>($"Unknown type: {typename}");
        }

        return Result.Success<object?>(type!);
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var match = _typeOfRegEx.Match(context.Expression);
        if (!match.Success)
        {
            return new ExpressionParseResultBuilder()
                .WithStatus(ResultStatus.Continue)
                .WithExpressionType(typeof(TypeOfExpression))
                .WithSourceExpression(context.Expression);
        }

        var typename = match.Groups["typename"].Value;

        var type = Type.GetType(typename, false);
        if (type is null)
        {
            return new ExpressionParseResultBuilder()
                .WithStatus(ResultStatus.Invalid)
                .WithErrorMessage($"Unknown type: {typename}")
                .WithExpressionType(typeof(TypeOfExpression))
                .WithSourceExpression(context.Expression);
        }

        return new ExpressionParseResultBuilder()
            .WithStatus(ResultStatus.Ok)
            .WithExpressionType(typeof(TypeOfExpression))
            .WithSourceExpression(context.Expression)
            .WithResultType(typeof(Type));

    }
}
