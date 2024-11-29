namespace CrossCutting.Utilities.Parsers;

public partial record FormattableStringParserSettings : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(PlaceholderStart) && PlaceholderStart == PlaceholderEnd)
        {
            yield return new ValidationResult($"{nameof(PlaceholderStart)} and {nameof(PlaceholderEnd)} cannot have the same value", [nameof(PlaceholderStart), nameof(PlaceholderEnd)]);
        }

        if (PlaceholderStart?.Contains("^") == true)
        {
            yield return new ValidationResult($"{nameof(PlaceholderStart)} cannot contain the ^ character", [nameof(PlaceholderStart)]);
        }

        if (PlaceholderEnd?.Contains("^") == true)
        {
            yield return new ValidationResult($"{nameof(PlaceholderEnd)} cannot contain the ^ character", [nameof(PlaceholderEnd)]);
        }
    }
}
