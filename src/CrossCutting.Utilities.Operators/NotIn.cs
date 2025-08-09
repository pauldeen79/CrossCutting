namespace CrossCutting.Utilities.Operators;

public static class NotIn
{
    public static Result<bool> Evaluate(object? leftValue, object? rightValue, StringComparison? stringComparison)
        => In.Evaluate(leftValue, rightValue, stringComparison).Transform(value => !value);
}
