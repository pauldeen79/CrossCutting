namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Aggregators;

public class PowerAggregator : Aggregator
{
    public override Result<object> Aggregate(object value1, object value2)
        => NumericAggregator.Evaluate(value1, value2
            , (x, y) => x ^ y
            , (x, y) => x ^ y
            , (x, y) => x ^ y
            , (x, y) => x ^ y
            , (x, y) => Math.Pow(x, y)
            , (x, y) => Math.Pow(Convert.ToDouble(x), Convert.ToDouble(y))
            , (x, y) => Math.Pow(x, y));

    public PowerAggregator() : base('^', 1) { }
}
