namespace CrossCutting.Utilities.Operators;

public static class NotEqual
{
    public static Result<bool> Evaluate(object? leftValue, object? rightValue, StringComparison stringComparison)
        => Result.FromExistingResult(Equal.Evaluate(leftValue, rightValue, stringComparison), value => !value);
}
