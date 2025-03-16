namespace CrossCutting.Utilities.Parsers.Expressions;

public class NumericExpression : IExpression
{
    private static readonly Regex _floatingPointRegEx = new(@"^[0-9]*(?:\.[0-9]*)?$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));
    private static readonly Regex _wholeNumberRegEx = new("^[0-9]*$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));
    private static readonly Regex _longNumberRegEx = new("^[0-9]*L$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));
    private static readonly Regex _wholeDecimalRegEx = new("^[0-9]*M$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));
    private static readonly Regex _floatingPointDecimalRegEx = new(@"^[0-9]*(?:\.[0-9]*)?$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var isFloatingPoint = new Lazy<bool>(() => _floatingPointRegEx.IsMatch(context.Expression));
        var isWholeNumber = new Lazy<bool>(() => _wholeNumberRegEx.IsMatch(context.Expression));
        var isLongNumber = new Lazy<bool>(() => _longNumberRegEx.IsMatch(context.Expression));
        var isWholeDecimal = new Lazy<bool>(() => _wholeDecimalRegEx.IsMatch(context.Expression));
        var isFloatingPointDecimal = new Lazy<bool>(() => _floatingPointDecimalRegEx.IsMatch(context.Expression));

        if (isWholeNumber.Value && int.TryParse(context.Expression, NumberStyles.AllowDecimalPoint, context.Settings.FormatProvider, out var i))
        {
            return Result.Success<object?>(i);
        }

        if (isWholeNumber.Value && long.TryParse(context.Expression, NumberStyles.AllowDecimalPoint, context.Settings.FormatProvider, out var l1))
        {
            return Result.Success<object?>(l1);
        }

        if (isLongNumber.Value && long.TryParse(context.Expression.Substring(0, context.Expression.Length - 1), NumberStyles.AllowDecimalPoint, context.Settings.FormatProvider, out var l2))
        {
            return Result.Success<object?>(l2);
        }

        if (isFloatingPoint.Value && context.Expression.Contains('.') && decimal.TryParse(context.Expression, NumberStyles.AllowDecimalPoint, context.Settings.FormatProvider, out var d1))
        {
            return Result.Success<object?>(d1);
        }

        if ((isWholeDecimal.Value || isFloatingPointDecimal.Value) && decimal.TryParse(context.Expression.Substring(0, context.Expression.Length - 1), NumberStyles.AllowDecimalPoint, context.Settings.FormatProvider, out var d2))
        {
            return Result.Success<object?>(d2);
        }

        return Result.Continue<object?>();
    }

    public Result<Type> Validate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var isFloatingPoint = new Lazy<bool>(() => _floatingPointRegEx.IsMatch(context.Expression));
        var isWholeNumber = new Lazy<bool>(() => _wholeNumberRegEx.IsMatch(context.Expression));
        var isLongNumber = new Lazy<bool>(() => _longNumberRegEx.IsMatch(context.Expression));
        var isWholeDecimal = new Lazy<bool>(() => _wholeDecimalRegEx.IsMatch(context.Expression));
        var isFloatingPointDecimal = new Lazy<bool>(() => _floatingPointDecimalRegEx.IsMatch(context.Expression));

        if (isWholeNumber.Value && int.TryParse(context.Expression, NumberStyles.AllowDecimalPoint, context.Settings.FormatProvider, out _))
        {
            return Result.Success(typeof(int));
        }

        if (isWholeNumber.Value && long.TryParse(context.Expression, NumberStyles.AllowDecimalPoint, context.Settings.FormatProvider, out _))
        {
            return Result.Success(typeof(long));
        }

        if (isLongNumber.Value && long.TryParse(context.Expression.Substring(0, context.Expression.Length - 1), NumberStyles.AllowDecimalPoint, context.Settings.FormatProvider, out _))
        {
            return Result.Success(typeof(long));
        }

        if (isFloatingPoint.Value && context.Expression.Contains('.') && decimal.TryParse(context.Expression, NumberStyles.AllowDecimalPoint, context.Settings.FormatProvider, out _))
        {
            return Result.Success(typeof(decimal));
        }

        if ((isWholeDecimal.Value || isFloatingPointDecimal.Value) && decimal.TryParse(context.Expression.Substring(0, context.Expression.Length - 1), NumberStyles.AllowDecimalPoint, context.Settings.FormatProvider, out _))
        {
            return Result.Success(typeof(decimal));
        }

        return Result.Continue<Type>();
    }
}
