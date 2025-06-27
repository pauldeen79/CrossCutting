namespace CrossCutting.Utilities.QueryEvaluator.Operators;

public partial record NotEqualsOperator
{
    public override Result<bool> Evaluate(object? leftValue, object? rightValue)
        => NotEqual.Evaluate(leftValue, rightValue, StringComparison);
}
