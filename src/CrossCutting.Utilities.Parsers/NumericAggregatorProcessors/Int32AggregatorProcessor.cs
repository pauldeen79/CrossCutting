namespace CrossCutting.Utilities.Parsers.NumericAggregatorProcessors;

public class Int32AggregatorProcessor : INumericAggregatorProcessor<int>
{
    public Result<object?> Aggregate(object firstValue, object secondValue, Func<int, int, object?> aggregatorDelegate)
    {
        if (firstValue is not int i1)
        {
            return Result<object?>.Continue();
        }

        int i2;
        try
        {
            i2 = Convert.ToInt32(secondValue, CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            return Result<object?>.Invalid($"Could not convert SecondExpression to Int32. Error message: {ex.Message}");
        }

        try
        {
            return Result<object?>.Success(aggregatorDelegate.Invoke(i1, i2));
        }
        catch (Exception ex)
        {
            return Result<object?>.Error($"Aggregation failed. Error message: {ex.Message}");
        }
    }
}
