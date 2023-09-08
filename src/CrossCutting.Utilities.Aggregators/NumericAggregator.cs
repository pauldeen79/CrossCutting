namespace CrossCutting.Utilities.Aggregators;

public static class NumericAggregator
{
#pragma warning disable S107 // Methods should not have too many parameters
    public static Result<object?> Evaluate(object? firstExpression,
                                           object? secondExpression,
                                           IFormatProvider formatProvider,
                                           Func<byte, byte, object> byteAggregatorDelegate,
                                           Func<short, short, object> shortAggregatorDelegate,
                                           Func<int, int, object> intAggregatorDelegate,
                                           Func<long, long, object> longAggregatorDelegate,
                                           Func<float, float, object> singleAggregatorDelegate,
                                           Func<decimal, decimal, object> decimalAggregatorDelegate,
                                           Func<double, double, object> doubleAggregatorDelegate)
#pragma warning restore S107 // Methods should not have too many parameters
        => (new Func<Result<object?>>[]
        {
            () => ByteAggregatorProcessor.Aggregate(firstExpression, secondExpression, formatProvider, byteAggregatorDelegate),
            () => Int16AggregatorProcessor.Aggregate(firstExpression, secondExpression, formatProvider, shortAggregatorDelegate),
            () => Int32AggregatorProcessor.Aggregate(firstExpression, secondExpression, formatProvider, intAggregatorDelegate),
            () => Int64AggregatorProcessor.Aggregate(firstExpression, secondExpression, formatProvider, longAggregatorDelegate),
            () => SingleAggregatorProcessor.Aggregate(firstExpression, secondExpression, formatProvider, singleAggregatorDelegate),
            () => DoubleAggregatorProcessor.Aggregate(firstExpression, secondExpression, formatProvider, doubleAggregatorDelegate),
            () => DecimalAggregatorProcessor.Aggregate(firstExpression, secondExpression, formatProvider, decimalAggregatorDelegate),
            () => Result<object?>.Invalid("First expression is not of a supported type")
        }).Select(FailSafe).First(x => x.Status != ResultStatus.Continue);

    private static Result<object?> FailSafe(Func<Result<object?>> x)
    {
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            return x.Invoke();
        }
        catch (Exception ex)
        {
            return Result<object?>.Error($"Aggregation failed. Error message: {ex.Message}");
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }
}
