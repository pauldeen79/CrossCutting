namespace CrossCutting.Utilities.Operators;

public static class SmallerThan
{
    public static Result<bool> Evaluate(object? leftValue, object? rightValue)
        => Comparable.Evaluate(leftValue, rightValue, result => result < 0);
}
