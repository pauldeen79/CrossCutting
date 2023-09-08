namespace CrossCutting.Common;

public static class ArgumentGuard
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T IsNotNull<T>(this T? instance, string name)
    {
        if (instance is null)
        {
            throw new ArgumentNullException(name);
        }

        return instance;
    }
}
