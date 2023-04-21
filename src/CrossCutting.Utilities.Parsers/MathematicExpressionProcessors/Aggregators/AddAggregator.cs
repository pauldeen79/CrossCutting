namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Aggregators;

internal class AddAggregator : Aggregator
{
    internal override Result<object> Aggregate(object value1, object value2)
        => NumericAggregator.Evaluate(value1, value2
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y);

    internal AddAggregator() : base('+', 3) { }
}
