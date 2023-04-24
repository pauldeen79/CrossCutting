namespace CrossCutting.Utilities.Parsers.NumericAggregatorProcessors;

public class SingleAggregatorProcessor : INumericAggregatorProcessor<float>
{
    public Result<object?> Aggregate(object firstValue, object secondValue, Func<float, float, object?> aggregatorDelegate)
    {
        if (firstValue is not float f1)
        {
            return Result<object?>.NotSupported();
        }

        float f2;
        try
        {
            f2 = Convert.ToSingle(secondValue, CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            return Result<object?>.Invalid($"Could not convert SecondExpression to Single. Error message: {ex.Message}");
        }

        try
        {
            return Result<object?>.Success(aggregatorDelegate.Invoke(f1, f2));
        }
        catch (Exception ex)
        {
            return Result<object?>.Error($"Aggregation failed. Error message: {ex.Message}");
        }
    }
}
