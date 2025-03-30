namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IAggregator
{
    Result<object?> Aggregate(object value1, object value2, IFormatProvider formatProvider);
}
