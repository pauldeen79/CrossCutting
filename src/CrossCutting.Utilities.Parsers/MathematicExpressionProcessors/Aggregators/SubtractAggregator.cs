namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Aggregators;

public class SubtractAggregator : AggregatorBase
{
    public override Result<object?> Aggregate(object value1, object value2, IFormatProvider formatProvider)
        => Subtract.Evaluate(value1, value2, formatProvider);

    public SubtractAggregator() : base('-', 3) { }
}
