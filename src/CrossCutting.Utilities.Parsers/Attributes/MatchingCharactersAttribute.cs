namespace CrossCutting.Utilities.Parsers.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class MatchingCharactersAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Check if the value is null or empty
        if (value is null || string.IsNullOrEmpty(value.ToString()))
        {
            return ValidationResult.Success; // Consider null or empty as valid
        }

        var input = value.ToString() ?? "";

        // Check if the input length is exactly 1 or 2
        if (input.Length is 1 or 2 && input.Distinct().Count() == 1)
        {
            return ValidationResult.Success;
        }

        // Validation failed
        return new ValidationResult($"The value '{input}' must consist of exactly one or two identical characters.");
    }
}
