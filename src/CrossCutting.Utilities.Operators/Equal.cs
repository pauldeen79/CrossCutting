namespace CrossCutting.Utilities.Operators;

public static class Equal
{
    public static Result<bool> Evaluate(object? leftValue, object? rightValue, StringComparison? stringComparison)
    {
        if (leftValue is null && rightValue is null)
        {
            return true;
        }

        if (leftValue is null || rightValue is null)
        {
            return false;
        }

        if (stringComparison is not null && leftValue is string leftString && rightValue is string rightString)
        {
            return leftString.Equals(rightString, stringComparison.Value);
        }

        return leftValue.Equals(rightValue);
    }
}
