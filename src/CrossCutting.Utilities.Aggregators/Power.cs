namespace CrossCutting.Utilities.Aggregators;

public static class Power
{
    public static Result<object?> Evaluate(object value1, object value2, IFormatProvider formatProvider)
        => NumericAggregator.Evaluate(value1, value2, formatProvider
            , (bas, exp) => Enumerable.Repeat(bas, exp).Aggregate(1, (a, b) => a * b)
            , (bas, exp) => Enumerable.Repeat(bas, exp).Aggregate(1, (a, b) => a * b)
            , (bas, exp) => Enumerable.Repeat(bas, exp).Aggregate(1, (a, b) => a * b)
            , (bas, exp) => Enumerable.Repeat(bas, Convert.ToInt32(exp, formatProvider)).Aggregate(1L, (a, b) => a * b)
            , (bas, exp) => Math.Pow(bas, exp)
            , (bas, exp) => Enumerable.Repeat(bas, Convert.ToInt32(exp, formatProvider)).Aggregate((decimal)1, (a, b) => a * b)
            , (bas, exp) => Math.Pow(bas, exp));
}
