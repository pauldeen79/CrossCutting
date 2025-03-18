namespace CrossCutting.Utilities.Parsers.MathematicExpressions.Aggregators;

public class BinaryAndAggregator() : AggregatorBase('&', 0)
{
    public override Result<object?> Aggregate(object value1, object value2, IFormatProvider formatProvider)
        => value1 is bool b1 && value2 is bool b2
            ? Result.Success<object?>(b1 && b2)
            : Result.Invalid<object?>("Binary and (&) operator can only be used on boolean values");
}
