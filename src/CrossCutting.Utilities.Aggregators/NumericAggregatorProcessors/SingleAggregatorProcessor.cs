namespace CrossCutting.Utilities.Aggregators;

public static class SingleAggregatorProcessor
{
    public static Result<object?> Aggregate(object firstValue, object secondValue, IFormatProvider formatProvider, Func<float, float, object?> aggregatorDelegate)
    {
        if (firstValue is not float f1)
        {
            return Result<object?>.Continue();
        }

        float f2;
        try
        {
            f2 = Convert.ToSingle(secondValue, formatProvider);
        }
        catch (Exception ex)
        {
            return Result<object?>.Invalid($"Could not convert SecondExpression to Single. Error message: {ex.Message}");
        }

        try
        {
            return Result<object?>.Success(aggregatorDelegate.Invoke(f1, f2));
        }
        catch (Exception ex)
        {
            return Result<object?>.Error($"Aggregation failed. Error message: {ex.Message}");
        }
    }
}
