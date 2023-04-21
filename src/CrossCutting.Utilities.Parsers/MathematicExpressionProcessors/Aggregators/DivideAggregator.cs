namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Aggregators;

internal class DivideAggregator : Aggregator
{
    internal override Result<object> Aggregate(object value1, object value2)
        => NumericAggregator.Evaluate(value1, value2
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y);

    internal DivideAggregator() : base('/', 2) { }
}
