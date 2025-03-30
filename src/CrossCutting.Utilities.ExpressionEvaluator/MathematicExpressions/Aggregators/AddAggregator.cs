namespace CrossCutting.Utilities.ExpressionEvaluator.MathematicExpressions.Aggregators;

public class AddAggregator() : AggregatorBase('+', 3)
{
    public override Result<object?> Aggregate(object value1, object value2, IFormatProvider formatProvider)
        => Add.Evaluate(value1, value2, formatProvider);
}
