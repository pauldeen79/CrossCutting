namespace CrossCutting.Utilities.Operators;

public static class Equal
{
    public static Result<bool> Evaluate(object? leftValue, object? rightValue, StringComparison stringComparison)
    {
        if (leftValue is null && rightValue is null)
        {
            return Result<bool>.Success(true);
        }

        if (leftValue is null || rightValue is null)
        {
            return Result<bool>.Success(false);
        }

        if (leftValue is string leftString && rightValue is string rightString)
        {
            return Result<bool>.Success(leftString.Equals(rightString, stringComparison));
        }

        return Result<bool>.Success(leftValue.Equals(rightValue));
    }
}
