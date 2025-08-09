namespace CrossCutting.Utilities.Parsers.MathematicExpressions.Aggregators;

public class PowerAggregator() : AggregatorBase('^', 1)
{
    public override Result<object?> Aggregate(object value1, object value2, IFormatProvider formatProvider)
        => Exponentiation.Evaluate(value1, value2, formatProvider);
}
