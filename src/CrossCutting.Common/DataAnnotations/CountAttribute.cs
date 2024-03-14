namespace CrossCutting.Common.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class CountAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is null)
        {
            // bypass validation in case of null
            return true;
        }

        if (value is IList list)
        {
            return list.Count >= MinimumCount && list.Count <= MaximumCount;
        }
        return false;
    }

    public int MinimumCount { get; }
    public int MaximumCount { get; }

    public CountAttribute(int minimumCount, int maximumCount)
    {
        MinimumCount = minimumCount;
        MaximumCount = maximumCount;
    }
}
