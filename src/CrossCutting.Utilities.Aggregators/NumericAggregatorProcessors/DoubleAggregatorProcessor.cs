namespace CrossCutting.Utilities.Aggregators;

public static class DoubleAggregatorProcessor
{
    public static Result<object?> Aggregate(object? firstValue, object? secondValue, IFormatProvider formatProvider, Func<double, double, object?> aggregatorDelegate)
    {
        if (firstValue is not double d1)
        {
            return Result<object?>.Continue();
        }

        var d2 = Convert.ToDouble(secondValue, formatProvider);
        return Result<object?>.Success(aggregatorDelegate.Invoke(d1, d2));
    }
}
