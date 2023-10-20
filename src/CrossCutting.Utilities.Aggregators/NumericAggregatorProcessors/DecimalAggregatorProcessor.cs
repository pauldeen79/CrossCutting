namespace CrossCutting.Utilities.Aggregators;

public static class DecimalAggregatorProcessor
{
    public static Result<object?> Aggregate(object? firstValue, object? secondValue, IFormatProvider formatProvider, Func<decimal, decimal, object?> aggregatorDelegate)
    {
        aggregatorDelegate = ArgumentGuard.IsNotNull(aggregatorDelegate, nameof(aggregatorDelegate));

        if (firstValue is not decimal d1)
        {
            return Result.Continue<object?>();
        }

        var d2 = Convert.ToDecimal(secondValue, formatProvider);
        return Result.Success(aggregatorDelegate.Invoke(d1, d2));
    }
}
