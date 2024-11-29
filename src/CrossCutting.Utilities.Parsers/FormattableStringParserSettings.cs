namespace CrossCutting.Utilities.Parsers;

public partial record FormattableStringParserSettings : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(PlaceholderStart) && PlaceholderStart == PlaceholderEnd)
        {
            yield return new ValidationResult($"{nameof(PlaceholderStart)} and {nameof(PlaceholderEnd)} cannot have the same value", [nameof(PlaceholderStart), nameof(PlaceholderEnd)]);
        }

        if (PlaceholderStart?.Contains("\uE002") == true)
        {
            yield return new ValidationResult($"{nameof(PlaceholderStart)} cannot contain the \uE002 character", [nameof(PlaceholderStart)]);
        }

        if (PlaceholderEnd?.Contains("\uE002") == true)
        {
            yield return new ValidationResult($"{nameof(PlaceholderEnd)} cannot contain the \uE002 character", [nameof(PlaceholderEnd)]);
        }
    }
}
