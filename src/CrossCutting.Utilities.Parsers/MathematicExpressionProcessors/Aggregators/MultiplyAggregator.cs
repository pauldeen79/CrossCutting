namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Aggregators;

internal class MultiplyAggregator : Aggregator
{
    internal override Result<object> Aggregate(object value1, object value2)
        => NumericAggregator.Evaluate(value1, value2
            , (x, y) => x * y
            , (x, y) => x * y
            , (x, y) => x * y
            , (x, y) => x * y
            , (x, y) => x * y
            , (x, y) => x * y
            , (x, y) => x * y);

    internal MultiplyAggregator() : base('*', 2) { }
}
