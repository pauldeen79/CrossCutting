namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Aggregators;

internal class PowerAggregator : Aggregator
{
    internal override Result<object> Aggregate(object value1, object value2)
        => NumericAggregator.Evaluate(value1, value2
            , (x, y) => x ^ y
            , (x, y) => x ^ y
            , (x, y) => x ^ y
            , (x, y) => x ^ y
            , (x, y) => Math.Pow(x, y)
            , (x, y) => Math.Pow(Convert.ToDouble(x), Convert.ToDouble(y))
            , (x, y) => Math.Pow(x, y));

    internal PowerAggregator() : base('^', 1) { }
}
