namespace CrossCutting.Utilities.Operators;

public static class Comparable
{
    public static Result<bool> Evaluate(object? leftValue, object? rightValue, Func<int, bool> compareResultDelegate)
    {
        compareResultDelegate = ArgumentGuard.IsNotNull(compareResultDelegate, nameof(compareResultDelegate));

        try
        {
            return Result.Success(leftValue is not null
                && rightValue is not null
                && leftValue is IComparable c
                && compareResultDelegate(c.CompareTo(rightValue)));
        }
        catch (ArgumentException ex)
        {
            return Result.Invalid<bool>(ex.Message);
        }
    }
}
