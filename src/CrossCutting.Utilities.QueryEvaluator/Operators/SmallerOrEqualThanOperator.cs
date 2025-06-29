namespace CrossCutting.Utilities.QueryEvaluator.Operators;

public partial record SmallerOrEqualThanOperator
{
    public override Result<bool> Evaluate(object? leftValue, object? rightValue)
        => SmallerOrEqualThan.Evaluate(leftValue, rightValue);
}
