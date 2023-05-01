namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Aggregators;

public class DivideAggregator : AggregatorBase
{
    public override Result<object?> Aggregate(object value1, object value2, IFormatProvider formatProvider)
        => Divide.Evaluate(value1, value2, formatProvider);

    public DivideAggregator() : base('/', 2) { }
}
