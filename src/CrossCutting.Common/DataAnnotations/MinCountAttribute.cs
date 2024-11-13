namespace CrossCutting.Common.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class MinCountAttribute(int count) : ValidationAttribute(DataAnnotationsResources.MinCountAttribute_ValidationError)
{
    public override bool IsValid(object value)
    {
        // Check the lengths for legality
        EnsureLegalLengths();

        if (value is null)
        {
            // bypass validation in case of null
            return true;
        }

        return value is IList list && list.Count >= Count;
    }

    public int Count { get; } = count;

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
    /// <exception cref="InvalidOperationException">Length is less than zero.</exception>
    private void EnsureLegalLengths()
    {
        if (Count < 0)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, DataAnnotationsResources.MinCountAttribute_InvalidMinLength));
        }
    }
}
