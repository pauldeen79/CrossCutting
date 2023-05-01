namespace CrossCutting.Utilities.Aggregators;

public static class ByteAggregatorProcessor
{
    public static Result<object?> Aggregate(object firstValue, object secondValue, IFormatProvider formatProvider, Func<byte, byte, object?> aggregatorDelegate)
    {
        if (firstValue is not byte b1)
        {
            return Result<object?>.Continue();
        }

        byte b2;
        try
        {
            b2 = Convert.ToByte(secondValue, formatProvider);
        }
        catch (Exception ex)
        {
            return Result<object?>.Invalid($"Could not convert SecondExpression to Byte. Error message: {ex.Message}");
        }

        try
        {
            return Result<object?>.Success(aggregatorDelegate.Invoke(b1, b2));
        }
        catch (Exception ex)
        {
            return Result<object?>.Error($"Aggregation failed. Error message: {ex.Message}");
        }
    }
}
