namespace CrossCutting.Utilities.QueryEvaluator.Operators;

public partial record IsNullOperator
{
    public override Result<bool> Evaluate(object? leftValue, object? rightValue)
        => Result.Success(leftValue is null);
}
