namespace CrossCutting.Utilities.Parsers.NumericAggregatorProcessors;

public class ByteAggregatorProcessor : INumericAggregatorProcessor<byte>
{
    public Result<object> Aggregate(object firstValue, object secondValue, Func<byte, byte, object> aggregatorDelegate)
    {
        if (firstValue is not byte b1)
        {
            return Result<object>.NotSupported();
        }

        byte b2;
        try
        {
            b2 = Convert.ToByte(secondValue, CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            return Result<object>.Invalid($"Could not convert SecondExpression to Byte. Error message: {ex.Message}");
        }

        try
        {
            return Result<object>.Success(aggregatorDelegate.Invoke(b1, b2));
        }
        catch (Exception ex)
        {
            return Result<object>.Error($"Aggregation failed. Error message: {ex.Message}");
        }
    }
}
