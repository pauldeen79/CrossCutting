namespace CrossCutting.Utilities.Parsers.MathematicExpressions.Aggregators;

public class DivideAggregator() : AggregatorBase('/', 2)
{
    public override Result<object?> Aggregate(object value1, object value2, IFormatProvider formatProvider)
        => Divide.Evaluate(value1, value2, formatProvider);
}
