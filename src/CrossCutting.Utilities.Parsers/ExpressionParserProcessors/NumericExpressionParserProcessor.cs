namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class NumericExpressionParserProcessor : IExpression
{
    private static readonly Regex _floatingPointRegEx = new(@"^[0-9]*(?:\.[0-9]*)?$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(200));
    private static readonly Regex _wholeNumberRegEx = new("^[0-9]*$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(200));
    private static readonly Regex _longNumberRegEx = new("^[0-9]*L$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(200));
    private static readonly Regex _wholeDecimalRegEx = new("^[0-9]*M$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(200));
    private static readonly Regex _floatingPointDecimalRegEx = new(@"^[0-9]*(?:\.[0-9]*)?$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(200));

    public int Order => 60;

    public Result<object?> Evaluate(string expression, IFormatProvider formatProvider, object? context)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return Result.Continue<object?>();
        }

        var isFloatingPoint = new Lazy<bool>(() => _floatingPointRegEx.IsMatch(expression));
        var isWholeNumber = new Lazy<bool>(() => _wholeNumberRegEx.IsMatch(expression));
        var isLongNumber = new Lazy<bool>(() => _longNumberRegEx.IsMatch(expression));
        var isWholeDecimal = new Lazy<bool>(() => _wholeDecimalRegEx.IsMatch(expression));
        var isFloatingPointDecimal = new Lazy<bool>(() => _floatingPointDecimalRegEx.IsMatch(expression));

        if (isWholeNumber.Value && int.TryParse(expression, NumberStyles.AllowDecimalPoint, formatProvider, out var i))
        {
            return Result.Success<object?>(i);
        }

        if (isWholeNumber.Value && long.TryParse(expression, NumberStyles.AllowDecimalPoint, formatProvider, out var l1))
        {
            return Result.Success<object?>(l1);
        }

        if (isLongNumber.Value && long.TryParse(expression.Substring(0, expression.Length - 1), NumberStyles.AllowDecimalPoint, formatProvider, out var l2))
        {
            return Result.Success<object?>(l2);
        }

        if (isFloatingPoint.Value && expression.Contains('.') && decimal.TryParse(expression, NumberStyles.AllowDecimalPoint, formatProvider, out var d1))
        {
            return Result.Success<object?>(d1);
        }

        if ((isWholeDecimal.Value || isFloatingPointDecimal.Value) && decimal.TryParse(expression.Substring(0, expression.Length - 1), NumberStyles.AllowDecimalPoint, formatProvider, out var d2))
        {
            return Result.Success<object?>(d2);
        }

        return Result.Continue<object?>();
    }

    public Result Validate(string expression, IFormatProvider formatProvider, object? context)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return Result.Continue<object?>();
        }

        var isFloatingPoint = new Lazy<bool>(() => _floatingPointRegEx.IsMatch(expression));
        var isWholeNumber = new Lazy<bool>(() => _wholeNumberRegEx.IsMatch(expression));
        var isLongNumber = new Lazy<bool>(() => _longNumberRegEx.IsMatch(expression));
        var isWholeDecimal = new Lazy<bool>(() => _wholeDecimalRegEx.IsMatch(expression));
        var isFloatingPointDecimal = new Lazy<bool>(() => _floatingPointDecimalRegEx.IsMatch(expression));

        if (isWholeNumber.Value && int.TryParse(expression, NumberStyles.AllowDecimalPoint, formatProvider, out _))
        {
            return Result.Success();
        }

        if (isWholeNumber.Value && long.TryParse(expression, NumberStyles.AllowDecimalPoint, formatProvider, out _))
        {
            return Result.Success();
        }

        if (isLongNumber.Value && long.TryParse(expression.Substring(0, expression.Length - 1), NumberStyles.AllowDecimalPoint, formatProvider, out _))
        {
            return Result.Success();
        }

        if (isFloatingPoint.Value && expression.Contains('.') && decimal.TryParse(expression, NumberStyles.AllowDecimalPoint, formatProvider, out _))
        {
            return Result.Success();
        }

        if ((isWholeDecimal.Value || isFloatingPointDecimal.Value) && decimal.TryParse(expression.Substring(0, expression.Length - 1), NumberStyles.AllowDecimalPoint, formatProvider, out _))
        {
            return Result.Success();
        }

        // Other values are ignored, so the expression parser knows whether an expression is supported
        return Result.Continue();
    }
}
