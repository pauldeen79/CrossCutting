namespace CrossCutting.Utilities.ExpressionEvaluator.Extensions;

public static class ObjectExtensions
{
    public static bool ToBoolean(this object? value)
    {
        if (value is bool b)
        {
            return b;
        }
        else if (value is string s)
        {
            // design decision: if it's a string, then do a null or empty check
            return !string.IsNullOrEmpty(s);
        }
        else
        {
            // design decision: if it's not a boolean, then do a null check
            return value is not null;
        }
    }
}
