namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Aggregators;

public class AddAggregator : AggregatorBase
{
    public override Result<object?> Aggregate(object value1, object value2, IFormatProvider formatProvider)
        => Add.Evaluate(value1, value2, formatProvider);

    public AddAggregator() : base('+', 3) { }
}
