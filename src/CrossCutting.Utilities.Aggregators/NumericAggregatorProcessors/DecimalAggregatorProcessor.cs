namespace CrossCutting.Utilities.Aggregators;

public static class DecimalAggregatorProcessor
{
    public static Result<object?> Aggregate(object firstValue, object secondValue, IFormatProvider formatProvider, Func<decimal, decimal, object?> aggregatorDelegate)
    {
        if (firstValue is not decimal d1)
        {
            return Result<object?>.Continue();
        }

        decimal d2;
        try
        {
            d2 = Convert.ToDecimal(secondValue, formatProvider);
        }
        catch (Exception ex)
        {
            return Result<object?>.Invalid($"Could not convert SecondExpression to Decimal. Error message: {ex.Message}");
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
