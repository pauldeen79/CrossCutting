namespace CrossCutting.Utilities.Parsers.NumericAggregatorProcessors;

public class DecimalAggregatorProcessor : INumericAggregatorProcessor<decimal>
{
    public Result<object?> Aggregate(object firstValue, object secondValue, Func<decimal, decimal, object?> aggregatorDelegate)
    {
        if (firstValue is not decimal d1)
        {
            return Result<object?>.NotSupported();
        }

        decimal d2;
        try
        {
            d2 = Convert.ToDecimal(secondValue, CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            return Result<object?>.Invalid($"Could not convert SecondExpression to Decimal. Error message: {ex.Message}");
        }

        try
        {
            return Result<object?>.Success(aggregatorDelegate.Invoke(d1, d2));
        }
        catch (Exception ex)
        {
            return Result<object?>.Error($"Aggregation failed. Error message: {ex.Message}");
        }
    }
}
