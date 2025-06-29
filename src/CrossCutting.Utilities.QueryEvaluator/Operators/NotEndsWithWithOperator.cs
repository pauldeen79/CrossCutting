namespace CrossCutting.Utilities.QueryEvaluator.Operators;

public partial record NotEndsWithOperator
{
    public override Result<bool> Evaluate(object? leftValue, object? rightValue)
        => leftValue is string leftString && rightValue is string rightString
            ? Result.Success(!leftString.EndsWith(rightString, StringComparison))
            : Result.Invalid<bool>("LeftValue and RightValue both need to be of type string");
}
