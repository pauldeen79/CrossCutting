namespace CrossCutting.Utilities.Aggregators;

public static class Subtract
{
    public static Result<object?> Evaluate(object? value1, object? value2, IFormatProvider formatProvider)
        => value1 is DateTime dt1 && value2 is DateTime dt2
            ? Result.Success<object?>(dt1 - dt2)
            : NumericAggregator.Evaluate(value1, value2, formatProvider
                , (x, y) => x - y
                , (x, y) => x - y
                , (x, y) => x - y
                , (x, y) => x - y
                , (x, y) => x - y
                , (x, y) => x - y
                , (x, y) => x - y);
}
