namespace CrossCutting.Common.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class CountAttribute : ValidationAttribute
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

        if (value is IList list)
        {
            return list.Count >= MinimumCount && list.Count <= MaximumCount;
        }

        return false;
    }

    public int MinimumCount { get; }
    public int MaximumCount { get; }

    public CountAttribute(int minimumCount, int maximumCount) : this()
    {
        MinimumCount = minimumCount;
        MaximumCount = maximumCount;
    }

    /// <summary>
    /// Override of <see cref="ValidationAttribute.FormatErrorMessage"/>
    /// </summary>
    /// <param name="name">The name to include in the formatted string</param>
    /// <returns>A localized string to describe the maximum acceptable length</returns>
    /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
    public override string FormatErrorMessage(string name)
    {
        EnsureLegalLengths();

        bool useErrorMessageWithMinimum = MinimumCount != 0;

        string errorMessage = useErrorMessageWithMinimum
            ? DataAnnotationsResources.CountAttribute_ValidationErrorIncludingMinimum
            : ErrorMessageString;

        // it's ok to pass in the minCount even for the error message without a {2} param since String.Format will just
        // ignore extra arguments
        return string.Format(CultureInfo.CurrentCulture, errorMessage, name, MaximumCount, MinimumCount);
    }

    /// <summary>
    /// Checks that MinimumCount and MaximumCount have legal values.  Throws InvalidOperationException if not.
    /// </summary>
    private void EnsureLegalLengths()
    {
        if (MaximumCount < 0)
        {
            throw new InvalidOperationException(DataAnnotationsResources.CountAttribute_InvalidMaxCount);
        }

        if (MaximumCount < MinimumCount)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, DataAnnotationsResources.CountAttribute_MinGreaterThanMax, MaximumCount, MinimumCount));
        }
    }

    private CountAttribute()
        : base(() => DataAnnotationsResources.CountAttribute_ValidationError)
    {
    }
}
