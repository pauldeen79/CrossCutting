namespace CrossCutting.Utilities.Aggregators;

public static class Int32AggregatorProcessor
{
    public static Result<object?> Aggregate(object? firstValue, object? secondValue, IFormatProvider formatProvider, Func<int, int, object?> aggregatorDelegate)
    {
        if (firstValue is not int i1)
        {
            return Result<object?>.Continue();
        }

        var i2 = Convert.ToInt32(secondValue, formatProvider);
        return Result<object?>.Success(aggregatorDelegate.Invoke(i1, i2));
    }
}
