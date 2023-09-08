namespace CrossCutting.Utilities.Aggregators;

public static class Int16AggregatorProcessor
{
    public static Result<object?> Aggregate(object? firstValue, object? secondValue, IFormatProvider formatProvider, Func<short, short, object?> aggregatorDelegate)
    {
        aggregatorDelegate = ArgumentGuard.IsNotNull(aggregatorDelegate, nameof(aggregatorDelegate));

        if (firstValue is not short s1)
        {
            return Result<object?>.Continue();
        }

        var s2 = Convert.ToInt16(secondValue, formatProvider);
        return Result<object?>.Success(aggregatorDelegate.Invoke(s1, s2));
    }
}
