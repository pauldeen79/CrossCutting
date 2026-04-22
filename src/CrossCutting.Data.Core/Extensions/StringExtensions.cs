namespace CrossCutting.Data.Core.Extensions;

public static class StringExtensions
{
    public static string FormatAsDatabaseIdentifier(this string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        if (value!.Contains("[") && value.Contains("]"))
        {
            return value;
        }

        return $"[{value}]";
    }
}