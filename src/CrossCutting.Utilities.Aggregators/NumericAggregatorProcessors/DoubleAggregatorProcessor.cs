namespace CrossCutting.Utilities.Aggregators;

public static class DoubleAggregatorProcessor
{
    public static Result<object?> Aggregate(object firstValue, object secondValue, IFormatProvider formatProvider, Func<double, double, object?> aggregatorDelegate)
    {
        if (firstValue is not double d1)
        {
            return Result<object?>.Continue();
        }

        double d2;
        try
        {
            d2 = Convert.ToDouble(secondValue, formatProvider);
        }
        catch (Exception ex)
        {
            return Result<object?>.Invalid($"Could not convert SecondExpression to Double. Error message: {ex.Message}");
        }

        try
        {
            return Result<object?>.Success(aggregatorDelegate.Invoke(d1, d2));
        }
        catch (Exception ex)
        {
            return Result<object?>.Error($"Aggregation failed. Error message: {ex.Message}");
        }
    }
}
