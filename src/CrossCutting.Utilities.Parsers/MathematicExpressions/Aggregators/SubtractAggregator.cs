namespace CrossCutting.Utilities.Parsers.MathematicExpressions.Aggregators;

public class SubtractAggregator() : AggregatorBase('-', 3)
{
    public override Result<object?> Aggregate(object value1, object value2, IFormatProvider formatProvider)
        => Subtract.Evaluate(value1, value2, formatProvider);
}
