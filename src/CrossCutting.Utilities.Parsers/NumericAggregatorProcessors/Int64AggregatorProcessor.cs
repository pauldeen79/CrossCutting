namespace CrossCutting.Utilities.Parsers.NumericAggregatorProcessors;

public class Int64AggregatorProcessor : INumericAggregatorProcessor<long>
{
    public Result<object> Aggregate(object firstValue, object secondValue, Func<long, long, object> aggregatorDelegate)
    {
        if (firstValue is not long l1)
        {
            return Result<object>.NotSupported();
        }

        long l2;
        try
        {
            l2 = Convert.ToInt64(secondValue, CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            return Result<object>.Invalid($"Could not convert SecondExpression to Int64. Error message: {ex.Message}");
        }

        try
        {
            return Result<object>.Success(aggregatorDelegate.Invoke(l1, l2));
        }
        catch (Exception ex)
        {
            return Result<object>.Error($"Aggregation failed. Error message: {ex.Message}");
        }
    }
}
