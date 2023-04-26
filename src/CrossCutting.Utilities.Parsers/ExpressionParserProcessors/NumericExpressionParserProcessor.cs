namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class NumericExpressionParserProcessor : IExpressionParserProcessor
{
    private static readonly Regex _floatingPointRegEx = new(@"^[0-9]*(?:\.[0-9]*)?$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(200));
    private static readonly Regex _wholeNumberRegEx = new("^[0-9]*$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(200));

    public int Order => 50;

    public Result<object?> Parse(string value, IFormatProvider formatProvider, object? context)
    {
        var isFloatingPoint = new Lazy<bool>(() => _floatingPointRegEx.IsMatch(value));
        var isWholeNumber = new Lazy<bool>(() => _wholeNumberRegEx.IsMatch(value));

        if (isFloatingPoint.Value && value.Contains('.') && decimal.TryParse(value, NumberStyles.AllowDecimalPoint, formatProvider, out var d))
        {
            return Result<object?>.Success(d);
        }

        if (isWholeNumber.Value && int.TryParse(value, NumberStyles.AllowDecimalPoint, formatProvider, out var i))
        {
            return Result<object?>.Success(i);
        }

        if (isWholeNumber.Value && long.TryParse(value, NumberStyles.AllowDecimalPoint, formatProvider, out var l))
        {
            return Result<object?>.Success(l);
        }

        return Result<object?>.Continue();
    }
}
