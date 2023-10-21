namespace CrossCutting.Utilities.Aggregators;

public static class DoubleAggregatorProcessor
{
    public static Result<object?> Aggregate(object? firstValue, object? secondValue, IFormatProvider formatProvider, Func<double, double, object?> aggregatorDelegate)
    {
        aggregatorDelegate = ArgumentGuard.IsNotNull(aggregatorDelegate, nameof(aggregatorDelegate));

        if (firstValue is not double d1)
        {
            return Result.Continue<object?>();
        }

        var d2 = Convert.ToDouble(secondValue, formatProvider);
        return Result.Success(aggregatorDelegate.Invoke(d1, d2));
    }
}
