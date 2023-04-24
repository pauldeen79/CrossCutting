namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Aggregators;

public class PowerAggregator : AggregatorBase
{
    public override Result<object?> Aggregate(object value1, object value2)
        => NumericAggregator.Evaluate(value1, value2
            , (bas, exp) => Enumerable.Repeat(bas, exp).Aggregate(1, (a, b) => a * b)
            , (bas, exp) => Enumerable.Repeat(bas, exp).Aggregate(1, (a, b) => a * b)
            , (bas, exp) => Enumerable.Repeat(bas, exp).Aggregate(1, (a, b) => a * b)
            , (bas, exp) => Enumerable.Repeat(bas, Convert.ToInt32(exp)).Aggregate(1L, (a, b) => a * b)
            , (bas, exp) => Math.Pow(bas, exp)
            , (bas, exp) => Enumerable.Repeat(bas, Convert.ToInt32(exp)).Aggregate((decimal)1, (a, b) => a * b)
            , (bas, exp) => Math.Pow(bas, exp));

    public PowerAggregator() : base('^', 1) { }
}
