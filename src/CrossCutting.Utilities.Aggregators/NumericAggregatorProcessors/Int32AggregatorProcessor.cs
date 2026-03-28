namespace CrossCutting.Utilities.Aggregators;

public static class Int32AggregatorProcessor
{
    public static Result<object?> Aggregate(object? firstValue, object? secondValue, IFormatProvider formatProvider, Func<int, int, object?> aggregatorDelegate)
    {
        aggregatorDelegate = ArgumentGuard.IsNotNull(aggregatorDelegate, nameof(aggregatorDelegate));

        if (firstValue is not int i1)
        {
            return Result.Continue<object?>();
        }

        var i2 = Convert.ToInt32(secondValue, formatProvider);
        return Result.Success(aggregatorDelegate.Invoke(i1, i2));
    }
}
