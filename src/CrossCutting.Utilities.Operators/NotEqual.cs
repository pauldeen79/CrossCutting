namespace CrossCutting.Utilities.Operators;

public static class NotEqual
{
    public static Result<bool> Evaluate(object? leftValue, object? rightValue, StringComparison? stringComparison)
        => Equal.Evaluate(leftValue, rightValue, stringComparison).Transform(value => !value);
}
