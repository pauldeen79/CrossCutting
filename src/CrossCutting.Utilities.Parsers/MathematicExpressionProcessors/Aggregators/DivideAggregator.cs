namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Aggregators;

public class DivideAggregator : Aggregator
{
    public override Result<object> Aggregate(object value1, object value2)
        => NumericAggregator.Evaluate(value1, value2
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y);

    public DivideAggregator() : base('/', 2) { }
}
