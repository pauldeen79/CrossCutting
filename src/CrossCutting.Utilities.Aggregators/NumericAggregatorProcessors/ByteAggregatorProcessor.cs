namespace CrossCutting.Utilities.Aggregators;

public static class ByteAggregatorProcessor
{
    public static Result<object?> Aggregate(object? firstValue, object? secondValue, IFormatProvider formatProvider, Func<byte, byte, object?> aggregatorDelegate)
    {
        aggregatorDelegate = ArgumentGuard.IsNotNull(aggregatorDelegate, nameof(aggregatorDelegate));

        if (firstValue is not byte b1)
        {
            return Result.Continue<object?>();
        }

        var b2 = Convert.ToByte(secondValue, formatProvider);
        return Result.Success(aggregatorDelegate.Invoke(b1, b2));
    }
}
