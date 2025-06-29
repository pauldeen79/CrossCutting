namespace CrossCutting.Utilities.QueryEvaluator.Operators;

public partial record NotContainsOperator
{
    public override Result<bool> Evaluate(object? leftValue, object? rightValue)
        => leftValue is string leftString && rightValue is string rightString
            ? Result.Success(leftString.IndexOf(rightString, StringComparison) == -1)
            : Result.Invalid<bool>("LeftValue and RightValue both need to be of type string");
}
