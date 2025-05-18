namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class DecimalExpressionComponent : IExpressionComponent
{
    private static readonly Regex _floatingPointRegEx = new(@"^[+-]?[0-9]*(?:\.[0-9]*)?$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));
    private static readonly Regex _wholeDecimalRegEx = new("^[+-]?[0-9]*M$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));
    private static readonly Regex _floatingPointDecimalRegEx = new(@"^[+-]?[0-9]*(?:\.[0-9]*)?$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));

    public int Order => 22;

    public Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.Run(() =>
        {
            context = ArgumentGuard.IsNotNull(context, nameof(context));

            var isFloatingPoint = new Lazy<bool>(() => _floatingPointRegEx.IsMatch(context.Expression));
            var isWholeDecimal = new Lazy<bool>(() => _wholeDecimalRegEx.IsMatch(context.Expression));
            var isFloatingPointDecimal = new Lazy<bool>(() => _floatingPointDecimalRegEx.IsMatch(context.Expression));

            if (isFloatingPoint.Value && decimal.TryParse(context.Expression, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out var d1))
            {
                return Result.Success<object?>(d1);
            }

            if ((isWholeDecimal.Value || isFloatingPointDecimal.Value) && decimal.TryParse(context.Expression.Substring(0, context.Expression.Length - 1), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out var d2))
            {
                return Result.Success<object?>(d2);
            }

            return Result.Continue<object?>();
        }, token);

    public Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.Run<ExpressionParseResult>(() =>
        {
            context = ArgumentGuard.IsNotNull(context, nameof(context));

            var isFloatingPoint = new Lazy<bool>(() => _floatingPointRegEx.IsMatch(context.Expression));
            var isWholeDecimal = new Lazy<bool>(() => _wholeDecimalRegEx.IsMatch(context.Expression));
            var isFloatingPointDecimal = new Lazy<bool>(() => _floatingPointDecimalRegEx.IsMatch(context.Expression));

            var type = default(Type?);

            if (isFloatingPoint.Value && context.Expression.Contains('.') && decimal.TryParse(context.Expression, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out _)
            || (isWholeDecimal.Value || isFloatingPointDecimal.Value) && decimal.TryParse(context.Expression.Substring(0, context.Expression.Length - 1), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out _))
            {
                type = typeof(decimal);
            }

            return new ExpressionParseResultBuilder()
                .WithStatus(type is null
                    ? ResultStatus.Continue
                    : ResultStatus.Ok)
                .WithExpressionComponentType(typeof(DecimalExpressionComponent))
                .WithSourceExpression(context.Expression)
                .WithResultType(type);
        }, token);
}
