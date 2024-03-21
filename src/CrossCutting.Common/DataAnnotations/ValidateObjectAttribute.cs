namespace CrossCutting.Common.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class ValidateObjectAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is null)
        {
            // Let RequiredValidationAttribute handle this
            return ValidationResult.Success;
        }

        var validationResults = new List<ValidationResult>();
        if (value is not string && value is IEnumerable e)
        {
            foreach (var item in e.OfType<object?>().Where(x => x is not null))
            {
                item!.TryValidate(validationResults);
            }
        }
        else
        {
            _ = value.TryValidate(validationResults);
        }

        if (validationResults.Count == 0)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult($"{validationContext?.MemberName}: {string.Join(Environment.NewLine, validationResults.Select(x => x.ErrorMessage))}", [validationContext?.MemberName]);
    }
}
