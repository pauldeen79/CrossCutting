namespace CrossCutting.Utilities.QueryEvaluator.Operators;

public partial record GreaterOrEqualThanOperator
{
    public override Result<bool> Evaluate(object? leftValue, object? rightValue)
        => GreaterOrEqualThan.Evaluate(leftValue, rightValue);
}
