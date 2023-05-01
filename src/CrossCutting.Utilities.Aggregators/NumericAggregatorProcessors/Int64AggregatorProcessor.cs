namespace CrossCutting.Utilities.Aggregators;

public static class Int64AggregatorProcessor
{
    public static Result<object?> Aggregate(object firstValue, object secondValue, IFormatProvider formatProvider, Func<long, long, object?> aggregatorDelegate)
    {
        if (firstValue is not long l1)
        {
            return Result<object?>.Continue();
        }

        var l2 = Convert.ToInt64(secondValue, formatProvider);
        return Result<object?>.Success(aggregatorDelegate.Invoke(l1, l2));
    }
}
