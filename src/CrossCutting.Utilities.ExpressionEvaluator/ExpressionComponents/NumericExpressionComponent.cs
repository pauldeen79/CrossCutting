namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class NumericExpressionComponent : IExpressionComponent
{
    private static readonly Regex _floatingPointRegEx = new(@"^[+-]?[0-9]*(?:\.[0-9]*)?$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));
    private static readonly Regex _wholeNumberRegEx = new("^[+-]?[0-9]*$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));
    private static readonly Regex _longNumberRegEx = new("^[+-]?[0-9]*L$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));
    private static readonly Regex _wholeDecimalRegEx = new("^[+-]?[0-9]*M$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));
    private static readonly Regex _floatingPointDecimalRegEx = new(@"^[+-]?[0-9]*(?:\.[0-9]*)?$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));

    public int Order => 14;

    public Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context)
        => Task.Run(() =>
        {
            context = ArgumentGuard.IsNotNull(context, nameof(context));

            var isFloatingPoint = new Lazy<bool>(() => _floatingPointRegEx.IsMatch(context.Expression));
            var isWholeNumber = new Lazy<bool>(() => _wholeNumberRegEx.IsMatch(context.Expression));
            var isLongNumber = new Lazy<bool>(() => _longNumberRegEx.IsMatch(context.Expression));
            var isWholeDecimal = new Lazy<bool>(() => _wholeDecimalRegEx.IsMatch(context.Expression));
            var isFloatingPointDecimal = new Lazy<bool>(() => _floatingPointDecimalRegEx.IsMatch(context.Expression));

            if (isWholeNumber.Value && int.TryParse(context.Expression, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out var i))
            {
                return Result.Success<object?>(i);
            }

            if (isWholeNumber.Value && long.TryParse(context.Expression, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out var l1))
            {
                return Result.Success<object?>(l1);
            }

            if (isLongNumber.Value && long.TryParse(context.Expression.Substring(0, context.Expression.Length - 1), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out var l2))
            {
                return Result.Success<object?>(l2);
            }

            if (isFloatingPoint.Value && decimal.TryParse(context.Expression, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out var d1))
            {
                return Result.Success<object?>(d1);
            }

            if ((isWholeDecimal.Value || isFloatingPointDecimal.Value) && decimal.TryParse(context.Expression.Substring(0, context.Expression.Length - 1), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out var d2))
            {
                return Result.Success<object?>(d2);
            }

            return Result.Continue<object?>();
        });

    public Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context)
        => Task.Run<ExpressionParseResult>(() =>
        {
            context = ArgumentGuard.IsNotNull(context, nameof(context));

            var isFloatingPoint = new Lazy<bool>(() => _floatingPointRegEx.IsMatch(context.Expression));
            var isWholeNumber = new Lazy<bool>(() => _wholeNumberRegEx.IsMatch(context.Expression));
            var isLongNumber = new Lazy<bool>(() => _longNumberRegEx.IsMatch(context.Expression));
            var isWholeDecimal = new Lazy<bool>(() => _wholeDecimalRegEx.IsMatch(context.Expression));
            var isFloatingPointDecimal = new Lazy<bool>(() => _floatingPointDecimalRegEx.IsMatch(context.Expression));

            var type = default(Type?);

            if (isWholeNumber.Value && int.TryParse(context.Expression, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out _))
            {
                type = typeof(int);
            }
            else if (isWholeNumber.Value && long.TryParse(context.Expression, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out _))
            {
                type = typeof(long);
            }
            else if (isLongNumber.Value && long.TryParse(context.Expression.Substring(0, context.Expression.Length - 1), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out _))
            {
                type = typeof(long);
            }
            else if (isFloatingPoint.Value && context.Expression.Contains('.') && decimal.TryParse(context.Expression, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out _))
            {
                type = typeof(decimal);
            }
            else if ((isWholeDecimal.Value || isFloatingPointDecimal.Value) && decimal.TryParse(context.Expression.Substring(0, context.Expression.Length - 1), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out _))
            {
                type = typeof(decimal);
            }

            return new ExpressionParseResultBuilder()
                .WithStatus(type is null
                    ? ResultStatus.Continue
                    : ResultStatus.Ok)
                .WithExpressionComponentType(typeof(NumericExpressionComponent))
                .WithSourceExpression(context.Expression)
                .WithResultType(type);
        });
}
