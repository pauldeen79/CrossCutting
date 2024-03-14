namespace CrossCutting.Common.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class MinCountAttribute : ValidationAttribute
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
            return list.Count >= Count;
        }
        return false;
    }

    public int Count { get; }

    public MinCountAttribute(int count) => Count = count;
}
