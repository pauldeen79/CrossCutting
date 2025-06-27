namespace CrossCutting.Utilities.QueryEvaluator.Operators;

public partial record SmallerThanOrEqualOperator
{
    public override Result<bool> Evaluate(object? leftValue, object? rightValue)
        => SmallerOrEqualThan.Evaluate(leftValue, rightValue);
}
