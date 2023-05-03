namespace CrossCutting.Utilities.Operators;

internal static class Comparable
{
    internal static Result<bool> Evaluate(object? leftValue, object? rightValue, Func<int, bool> compareResultDelegate)
    {
        try
        {
            return Result<bool>.Success(leftValue is not null
                && rightValue is not null
                && leftValue is IComparable c
                && compareResultDelegate(c.CompareTo(rightValue)));
        }
        catch (ArgumentException ex)
        {
            return Result<bool>.Invalid(ex.Message);
        }
    }
}
