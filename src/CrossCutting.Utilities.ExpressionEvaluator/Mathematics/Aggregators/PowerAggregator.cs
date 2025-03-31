namespace CrossCutting.Utilities.ExpressionEvaluator.Mathematics.Aggregators;

public class PowerAggregator() : AggregatorBase('^', 1)
{
    public override Result<object?> Aggregate(object value1, object value2, IFormatProvider formatProvider)
        => Power.Evaluate(value1, value2, formatProvider);
}
