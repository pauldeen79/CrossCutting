namespace CrossCutting.Utilities.Aggregators;

public static class DecimalAggregatorProcessor
{
    public static Result<object?> Aggregate(object? firstValue, object? secondValue, IFormatProvider formatProvider, Func<decimal, decimal, object?> aggregatorDelegate)
    {
        if (firstValue is not decimal d1)
        {
            return Result<object?>.Continue();
        }


        var d2 = Convert.ToDecimal(secondValue, formatProvider);
        return Result<object?>.Success(aggregatorDelegate.Invoke(d1, d2));
    }
}
