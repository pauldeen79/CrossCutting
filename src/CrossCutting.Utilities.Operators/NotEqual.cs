namespace CrossCutting.Utilities.Operators;

public static class NotEqual
{
    public static Result<bool> Evaluate(object? leftValue, object? rightValue)
        => Result<bool>.FromExistingResult(Equal.Evaluate(leftValue, rightValue, StringComparison.CurrentCultureIgnoreCase), value => !value);
}
