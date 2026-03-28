namespace CrossCutting.Utilities.Operators;

public static class In
{
    public static Result<bool> Evaluate(object? leftValue, object? rightValue, StringComparison? stringComparison)
        => Result.Success(leftValue is null
            ? ValueContainsNull(rightValue)
            : leftValue.In(stringComparison.GetValueOrDefault(StringComparison.CurrentCulture), GetValues(rightValue)));

    private static bool ValueContainsNull(object? value)
        => value is not string and IEnumerable enumerable
            ? GetValues(enumerable).Contains(null)
            : value is null;

    private static IEnumerable<object?> GetValues(object? value)
        => value is not string and IEnumerable enumerable
            ? GetValues(enumerable)
            : [value];

    private static IEnumerable<object?> GetValues(IEnumerable enumerable)
    {
        foreach (var item in enumerable)
        {
            yield return item;
        }
    }
}
