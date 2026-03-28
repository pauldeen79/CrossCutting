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
            () => Result.Invalid<object?>("First expression is not of a supported type")
        }).Select(x => Result.WrapException(x, "Aggregation failed. Error message: {0}")).First(x => x.Status != ResultStatus.Continue);

    public static Result<Type> Validate(Type? type1, Type? type2)
    {
        if (type1 is null)
        {
            return Result.NoContent<Type>();
        }

        if (type1 != type2)
        {
            return Result.NoContent<Type>();
        }

        if (type1 == typeof(byte))
        {
            return Result.Success(typeof(byte));
        }

        if (type1 == typeof(short))
        {
            return Result.Success(typeof(short));
        }

        if (type1 == typeof(int))
        {
            return Result.Success(typeof(int));
        }

        if (type1 == typeof(long))
        {
            return Result.Success(typeof(long));
        }

        if (type1 == typeof(float))
        {
            return Result.Success(typeof(float));
        }

        if (type1 == typeof(double))
        {
            return Result.Success(typeof(double));
        }

        if (type1 == typeof(decimal))
        {
            return Result.Success(typeof(decimal));
        }

        return Result.Invalid<Type>($"Unsupported type: {type1.FullName}");
    }
}
