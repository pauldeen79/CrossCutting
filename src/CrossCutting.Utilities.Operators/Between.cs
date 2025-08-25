namespace CrossCutting.Utilities.Operators;

public static class Between
{
    public static Result<bool> Evaluate(object? sourceValue, object? lowerBound, object? upperBound)
    {
        try
        {
            return Result.Success(sourceValue is not null
                && lowerBound is not null
                && upperBound is not null
                && sourceValue is IComparable c
                && c.CompareTo(lowerBound) >= 0
                && c.CompareTo(upperBound) <= 0);
        }
        catch (ArgumentException ex)
        {
            return Result.Invalid<bool>(ex.Message);
        }
    }
}
