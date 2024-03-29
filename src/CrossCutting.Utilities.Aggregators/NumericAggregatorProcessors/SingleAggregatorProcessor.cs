﻿namespace CrossCutting.Utilities.Aggregators;

public static class SingleAggregatorProcessor
{
    public static Result<object?> Aggregate(object? firstValue, object? secondValue, IFormatProvider formatProvider, Func<float, float, object?> aggregatorDelegate)
    {
        aggregatorDelegate = ArgumentGuard.IsNotNull(aggregatorDelegate, nameof(aggregatorDelegate));

        if (firstValue is not float f1)
        {
            return Result.Continue<object?>();
        }

        var f2 = Convert.ToSingle(secondValue, formatProvider);
        return Result.Success(aggregatorDelegate.Invoke(f1, f2));
    }
}
