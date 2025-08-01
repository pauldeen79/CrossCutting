namespace CrossCutting.Utilities.Aggregators;

public static class Exponentiation
{
    public static Result<object?> Evaluate(object? value1, object? value2, IFormatProvider formatProvider)
        => NumericAggregator.Evaluate(value1, value2, formatProvider
            , (bas, exp) => Math.Pow(bas, exp)
            , (bas, exp) => Math.Pow(bas, exp)
            , (bas, exp) => Math.Pow(bas, exp)
            , (bas, exp) => Math.Pow(bas, exp)
            , (bas, exp) => Math.Pow(bas, exp)
            , (bas, exp) => Math.Pow(Convert.ToDouble(bas), Convert.ToDouble(exp))
            , (bas, exp) => Math.Pow(bas, exp));
}
