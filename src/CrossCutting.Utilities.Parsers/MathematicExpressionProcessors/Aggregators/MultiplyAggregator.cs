namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Aggregators;

public class MultiplyAggregator : AggregatorBase
{
    public override Result<object?> Aggregate(object value1, object value2, IFormatProvider formatProvider)
        => Multiply.Evaluate(value1, value2, formatProvider);

    public MultiplyAggregator() : base('*', 2) { }
}
