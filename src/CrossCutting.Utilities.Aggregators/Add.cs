namespace CrossCutting.Utilities.Aggregators;

public static class Add
{
    public static Result<object?> Evaluate(object? value1, object? value2, IFormatProvider formatProvider)
        => value1 is string s1 && value2 is string s2
            ? Result.Success<object?>(s1 + s2)
            : NumericAggregator.Evaluate(value1, value2, formatProvider
                , (x, y) => x + y
                , (x, y) => x + y
                , (x, y) => x + y
                , (x, y) => x + y
                , (x, y) => x + y
                , (x, y) => x + y
                , (x, y) => x + y);
}
