namespace CrossCutting.Utilities.Parsers;

public static class NumericAggregator
{
#pragma warning disable S107 // Methods should not have too many parameters
    public static Result<object> Evaluate(object firstExpression,
                                          object secondExpression,
                                          Func<byte, byte, object> byteAggregatorDelegate,
                                          Func<short, short, object> shortAggregatorDelegate,
                                          Func<int, int, object> intAggregatorDelegate,
                                          Func<long, long, object> longAggregatorDelegate,
                                          Func<float, float, object> singleAggregatorDelegate,
                                          Func<decimal, decimal, object> decimalAggregatorDelegate,
                                          Func<double, double, object> doubleAggregatorDelegate)
#pragma warning restore S107 // Methods should not have too many parameters
        => new Func<Result<object>>[]
        {
            () => new ByteAggregatorProcessor().Aggregate(firstExpression, secondExpression, byteAggregatorDelegate),
            () => new Int16AggregatorProcessor().Aggregate(firstExpression, secondExpression, shortAggregatorDelegate),
            () => new Int32AggregatorProcessor().Aggregate(firstExpression, secondExpression, intAggregatorDelegate),
            () => new Int64AggregatorProcessor().Aggregate(firstExpression, secondExpression, longAggregatorDelegate),
            () => new SingleAggregatorProcessor().Aggregate(firstExpression, secondExpression, singleAggregatorDelegate),
            () => new DoubleAggregatorProcessor().Aggregate(firstExpression, secondExpression, doubleAggregatorDelegate),
            () => new DecimalAggregatorProcessor().Aggregate(firstExpression, secondExpression, decimalAggregatorDelegate),
            () => Result<object>.Invalid("First expression is not of a supported type")
        }.Select(x => x.Invoke()).First(x => x.Status != ResultStatus.NotSupported);
}
