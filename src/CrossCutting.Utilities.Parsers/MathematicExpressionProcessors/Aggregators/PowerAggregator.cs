namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Aggregators;

public class PowerAggregator : AggregatorBase
{
    public override Result<object?> Aggregate(object value1, object value2, IFormatProvider formatProvider)
        => Power.Evaluate(value1, value2, formatProvider);

    public PowerAggregator() : base('^', 1) { }
}
