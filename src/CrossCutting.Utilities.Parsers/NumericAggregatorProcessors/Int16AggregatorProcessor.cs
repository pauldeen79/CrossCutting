namespace CrossCutting.Utilities.Parsers.NumericAggregatorProcessors;

public class Int16AggregatorProcessor : INumericAggregatorProcessor<short>
{
    public Result<object?> Aggregate(object firstValue, object secondValue, Func<short, short, object?> aggregatorDelegate)
    {
        if (firstValue is not short s1)
        {
            return Result<object?>.NotSupported();
        }

        short s2;
        try
        {
            s2 = Convert.ToInt16(secondValue, CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            return Result<object?>.Invalid($"Could not convert SecondExpression to Int32. Error message: {ex.Message}");
        }

        try
        {
            return Result<object?>.Success(aggregatorDelegate.Invoke(s1, s2));
        }
        catch (Exception ex)
        {
            return Result<object?>.Error($"Aggregation failed. Error message: {ex.Message}");
        }
    }
}
