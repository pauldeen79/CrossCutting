namespace CrossCutting.Utilities.QueryEvaluator.Operators;

public partial record IsNotNullOperator
{
    public override Result<bool> Evaluate(object? leftValue, object? rightValue)
        => Result.Success(leftValue is not null);
}
