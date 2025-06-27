namespace CrossCutting.Utilities.QueryEvaluator.Operators;

public partial record GreaterThanOperator
{
    public override Result<bool> Evaluate(object? leftValue, object? rightValue)
        => GreaterThan.Evaluate(leftValue, rightValue);
}
