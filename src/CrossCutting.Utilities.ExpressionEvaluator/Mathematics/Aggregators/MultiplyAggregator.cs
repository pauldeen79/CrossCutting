namespace CrossCutting.Utilities.ExpressionEvaluator.Mathematics.Aggregators;

public class MultiplyAggregator() : AggregatorBase('*', 2)
{
    public override Result<object?> Aggregate(object value1, object value2, IFormatProvider formatProvider)
        => Multiply.Evaluate(value1, value2, formatProvider);
}
