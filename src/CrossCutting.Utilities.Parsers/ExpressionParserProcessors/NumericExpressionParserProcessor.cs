namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class NumericExpressionParserProcessor : IExpressionParserProcessor
{
    private static readonly Regex _floatingPointRegEx = new(@"^[0-9]*(?:\.[0-9]*)?$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(200));
    private static readonly Regex _wholeNumberRegEx = new("^[0-9]*$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(200));
    private static readonly Regex _longNumberRegEx = new("^[0-9]*L$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(200));
    private static readonly Regex _wholeDecimalRegEx = new("^[0-9]*M$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(200));
    private static readonly Regex _floatingPointDecimalRegEx = new(@"^[0-9]*(?:\.[0-9]*)?$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(200));

    public int Order => 60;

    public Result<object?> Parse(string value, IFormatProvider formatProvider, object? context)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));
        ArgumentGuard.IsNotNull(formatProvider, nameof(formatProvider));

        var isFloatingPoint = new Lazy<bool>(() => _floatingPointRegEx.IsMatch(value));
        var isWholeNumber = new Lazy<bool>(() => _wholeNumberRegEx.IsMatch(value));
        var isLongNumber = new Lazy<bool>(() => _longNumberRegEx.IsMatch(value));
        var isWholeDecimal = new Lazy<bool>(() => _wholeDecimalRegEx.IsMatch(value));
        var isFloatingPointDecimal = new Lazy<bool>(() => _floatingPointDecimalRegEx.IsMatch(value));

        if (isWholeNumber.Value && int.TryParse(value, NumberStyles.AllowDecimalPoint, formatProvider, out var i))
        {
            return Result<object?>.Success(i);
        }

        if (isWholeNumber.Value && long.TryParse(value, NumberStyles.AllowDecimalPoint, formatProvider, out var l1))
        {
            return Result<object?>.Success(l1);
        }

        if (isLongNumber.Value && long.TryParse(value.Substring(0, value.Length - 1), NumberStyles.AllowDecimalPoint, formatProvider, out var l2))
        {
            return Result<object?>.Success(l2);
        }

        if (isFloatingPoint.Value && value.Contains('.') && decimal.TryParse(value, NumberStyles.AllowDecimalPoint, formatProvider, out var d1))
        {
            return Result<object?>.Success(d1);
        }

        if ((isWholeDecimal.Value || isFloatingPointDecimal.Value) && decimal.TryParse(value.Substring(0, value.Length - 1), NumberStyles.AllowDecimalPoint, formatProvider, out var d2))
        {
            return Result<object?>.Success(d2);
        }

        return Result<object?>.Continue();
    }
}
