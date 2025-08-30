namespace CrossCutting.Common;

public static class ArgumentGuard
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static T IsNotNull<T>(this T? instance, string name)
    {
        if (instance is null)
        {
            throw new ArgumentNullException(name);
        }

        return instance;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static string IsNotNullOrEmpty(this string? instance, string name)
    {
        if (string.IsNullOrEmpty(instance))
        {
            throw new ArgumentException($"{name} cannot be null or empty", name);
        }

        return instance!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static string IsNotNullOrWhiteSpace(this string? instance, string name)
    {
        if (string.IsNullOrWhiteSpace(instance))
        {
            throw new ArgumentException($"{name} cannot be null or whitespace", name);
        }

        return instance!;
    }
}
