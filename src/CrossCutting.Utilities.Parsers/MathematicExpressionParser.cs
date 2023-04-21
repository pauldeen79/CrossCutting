namespace CrossCutting.Utilities.Parsers;

public static partial class MathematicExpressionParser
{
    internal const string TemporaryDelimiter = "``";

    private static readonly IMathematicExpressionProcessor[] _expressionProcessors = new IMathematicExpressionProcessor[]
    {
        new Validate(),
        new Recursion(),
        new Operators(),
    };

    public static Result<object> Parse(
        string input,
        IFormatProvider formatProvider,
        Func<string, IFormatProvider, Result<object>> parseExpressionDelegate)
    {
        var state = new MathematicExpressionState(input, formatProvider, parseExpressionDelegate, Parse);
        foreach (var processor in _expressionProcessors)
        {
            var result = processor.Process(state);
            if (!result.IsSuccessful())
            {
                return Result<object>.FromExistingResult(result);
            }
        }

        return state.Results.Any()
            ? state.Results.Last()
            : parseExpressionDelegate(input, formatProvider);
    }

    private static readonly Regex _floatingPointRegEx = new(@"^[0-9]*(?:\.[0-9]*)?$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(200));
    private static readonly Regex _wholeNumberRegEx = new("^[0-9]*$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(200));
    public static Result<object> DefaultParseExpressionDelegate(string value, IFormatProvider formatProvider)
    {
        var isFloatingPoint = new Lazy<bool>(() => _floatingPointRegEx.IsMatch(value));
        var isWholeNumber = new Lazy<bool>(() => _wholeNumberRegEx.IsMatch(value));

        if (value == "true")
        {
            return Result<object>.Success(true);
        }

        if (value == "false")
        {
            return Result<object>.Success(false);
        }

        if (value.StartsWith("\"") && value.EndsWith("\""))
        {
            return Result<object>.Success(value.Substring(1, value.Length - 2));
        }

        if (isFloatingPoint.Value && value.Contains('.') && decimal.TryParse(value, NumberStyles.AllowDecimalPoint, formatProvider, out var d))
        {
            return Result<object>.Success(d);
        }

        if (isWholeNumber.Value && int.TryParse(value, NumberStyles.AllowDecimalPoint, formatProvider, out var i))
        {
            return Result<object>.Success(i);
        }

        if (isWholeNumber.Value && long.TryParse(value, NumberStyles.AllowDecimalPoint, formatProvider, out var l))
        {
            return Result<object>.Success(l);
        }

        if (DateTime.TryParse(value, formatProvider, DateTimeStyles.None, out var dt))
        {
            return Result<object>.Success(dt);
        }

        // In other cases, it's unknown
        return Result<object>.Invalid($"Unknown expression type found in fragment: {value}");
    }
}
