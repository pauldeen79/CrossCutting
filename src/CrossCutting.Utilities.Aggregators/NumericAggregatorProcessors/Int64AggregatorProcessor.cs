namespace CrossCutting.Utilities.Aggregators;

public static class Int64AggregatorProcessor
{
    public static Result<object?> Aggregate(object firstValue, object secondValue, IFormatProvider formatProvider, Func<long, long, object?> aggregatorDelegate)
    {
        if (firstValue is not long l1)
        {
            return Result<object?>.Continue();
        }

        long l2;
        try
        {
            l2 = Convert.ToInt64(secondValue, formatProvider);
        }
        catch (Exception ex)
        {
            return Result<object?>.Invalid($"Could not convert SecondExpression to Int64. Error message: {ex.Message}");
        }

        try
        {
            return Result<object?>.Success(aggregatorDelegate.Invoke(l1, l2));
        }
        catch (Exception ex)
        {
            return Result<object?>.Error($"Aggregation failed. Error message: {ex.Message}");
        }
    }
}
