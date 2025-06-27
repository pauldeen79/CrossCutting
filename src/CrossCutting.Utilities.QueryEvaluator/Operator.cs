namespace CrossCutting.Utilities.QueryEvaluator;

public partial record Operator
{
    public abstract Result<bool> Evaluate(object? leftValue, object? rightValue);
}
