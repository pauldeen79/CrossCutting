namespace CrossCutting.Common.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class MaxCountAttribute : ValidationAttribute
{
    private const int MaxAllowableCount = -1;

    public override bool IsValid(object value)
    {
        // Check the lengths for legality
        EnsureLegalLengths();

        if (value is null)
        {
            // bypass validation in case of null
            return true;
        }

        return MaxAllowableCount == Count || (value is IList list && list.Count <= Count);
    }

    public int Count { get; }

    public MaxCountAttribute(int count) : base(() => DataAnnotationsResources.MaxCountAttribute_ValidationError)
    {
        Count = count;
    }

    public MaxCountAttribute() : base(() => DataAnnotationsResources.MaxCountAttribute_ValidationError)
    {
        Count = MaxAllowableCount;
    }

    /// <summary>
    /// Applies formatting to a specified error message. (Overrides <see cref = "ValidationAttribute.FormatErrorMessage" />)
    /// </summary>
    /// <param name = "name">The name to include in the formatted string.</param>
    /// <returns>A localized string to describe the maximum acceptable length.</returns>
    public override string FormatErrorMessage(string name)
    {
        // An error occurred, so we know the value is greater than the maximum if it was specified
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Count);
    }

    /// <summary>
    /// Checks that Length has a legal value.
    /// </summary>
    /// <exception cref="InvalidOperationException">Length is zero or less than negative one.</exception>
    private void EnsureLegalLengths()
    {
        if (Count is 0 or < (-1))
        {
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, DataAnnotationsResources.MaxCountAttribute_InvalidMaxCount));
        }
    }
}
