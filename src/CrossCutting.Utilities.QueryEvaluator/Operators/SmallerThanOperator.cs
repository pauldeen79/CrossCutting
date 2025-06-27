namespace CrossCutting.Utilities.QueryEvaluator.Operators;

public partial record SmallerThanOperator
{
    public override Result<bool> Evaluate(object? leftValue, object? rightValue)
        => SmallerThan.Evaluate(leftValue, rightValue);
}
