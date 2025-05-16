namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class TypeOfExpressionComponent : IExpressionComponent
{
    private static readonly Regex _typeOfRegEx = new(@"^typeof\((?<typename>.+?)\)$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));

    public int Order => 13;

    public Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context)
        => Task.Run(() =>
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
        });

    public Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context)
        => Task.Run<ExpressionParseResult>(() =>
        {
            context = ArgumentGuard.IsNotNull(context, nameof(context));

            var match = _typeOfRegEx.Match(context.Expression);
            if (!match.Success)
            {
                return new ExpressionParseResultBuilder()
                    .WithStatus(ResultStatus.Continue)
                    .WithExpressionComponentType(typeof(TypeOfExpressionComponent))
                    .WithSourceExpression(context.Expression);
            }

            var typename = match.Groups["typename"].Value;

            var type = Type.GetType(typename, false);
            if (type is null)
            {
                return new ExpressionParseResultBuilder()
                    .WithStatus(ResultStatus.Invalid)
                    .WithErrorMessage($"Unknown type: {typename}")
                    .WithExpressionComponentType(typeof(TypeOfExpressionComponent))
                    .WithSourceExpression(context.Expression);
            }

            return new ExpressionParseResultBuilder()
                .WithStatus(ResultStatus.Ok)
                .WithExpressionComponentType(typeof(TypeOfExpressionComponent))
                .WithSourceExpression(context.Expression)
                .WithResultType(typeof(Type));
        });
}
