namespace CrossCutting.Utilities.QueryEvaluator.Operators;

public partial record EqualsOperator
{
    public override Result<bool> Evaluate(object? leftValue, object? rightValue)
        => Equal.Evaluate(leftValue, rightValue, StringComparison);
}
