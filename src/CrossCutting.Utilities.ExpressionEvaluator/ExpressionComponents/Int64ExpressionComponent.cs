namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class Int64ExpressionComponent : IExpressionComponent
{
    private static readonly Regex _wholeNumberRegEx = new("^[+-]?[0-9]*$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));
    private static readonly Regex _longNumberRegEx = new("^[+-]?[0-9]*L$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));

    public int Order => 21;

    public Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.Run(() =>
        {
            context = ArgumentGuard.IsNotNull(context, nameof(context));

            var isWholeNumber = new Lazy<bool>(() => _wholeNumberRegEx.IsMatch(context.Expression));
            var isLongNumber = new Lazy<bool>(() => _longNumberRegEx.IsMatch(context.Expression));

            if (isWholeNumber.Value && long.TryParse(context.Expression, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out var l1))
            {
                return Result.Success<object?>(l1);
            }

            if (isLongNumber.Value && long.TryParse(context.Expression.Substring(0, context.Expression.Length - 1), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out var l2))
            {
                return Result.Success<object?>(l2);
            }

            return Result.Continue<object?>();
        }, token);

    public Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.Run<ExpressionParseResult>(() =>
        {
            context = ArgumentGuard.IsNotNull(context, nameof(context));

            var isWholeNumber = new Lazy<bool>(() => _wholeNumberRegEx.IsMatch(context.Expression));
            var isLongNumber = new Lazy<bool>(() => _longNumberRegEx.IsMatch(context.Expression));

            var type = default(Type?);

            if (isWholeNumber.Value && long.TryParse(context.Expression, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out _)
            || isLongNumber.Value && long.TryParse(context.Expression.Substring(0, context.Expression.Length - 1), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out _))
            {
                type = typeof(long);
            }

            return new ExpressionParseResultBuilder()
                .WithStatus(type is null
                    ? ResultStatus.Continue
                    : ResultStatus.Ok)
                .WithExpressionComponentType(typeof(Int64ExpressionComponent))
                .WithSourceExpression(context.Expression)
                .WithResultType(type);
        }, token);
}
