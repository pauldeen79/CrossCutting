namespace CrossCutting.Utilities.Aggregators;

public static class Add
{
    public static Result<object?> Evaluate(object? value1, object? value2, IFormatProvider formatProvider)
        => NumericAggregator.Evaluate(value1, value2, formatProvider
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y);
}
