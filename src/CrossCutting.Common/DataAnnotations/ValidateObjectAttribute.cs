namespace CrossCutting.Common.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class ValidateObjectAttribute : ValidationAttribute
{
    public bool DetailedErrorMessages { get; set; }

    public override bool IsValid(object value)
    {
        if (value is null)
        {
            // Let RequiredValidation handle this
            return true;
        }

        var validationResults = new List<ValidationResult>();

        if (value is not string and IEnumerable e)
        {
            foreach (var item in e.OfType<object?>())
            {
                if (!item!.TryValidate(validationResults))
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            return value.TryValidate(validationResults);
        }
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (!DetailedErrorMessages)
        {
            return base.IsValid(value, validationContext);
        }

        if (value is null)
        {
            // Let RequiredValidationAttribute handle this
            return ValidationResult.Success;
        }

        var validationResults = new List<ValidationResult>();
        if (value is not string and IEnumerable e)
        {
            foreach (var item in e.OfType<object?>())
            {
                _ = item!.TryValidate(validationResults);
            }
        }
        else
        {
            _ = value.TryValidate(validationResults);
        }

        return validationResults.Count == 0
            ? ValidationResult.Success
            : new ValidationResult($"{validationContext?.MemberName}: {string.Join(Environment.NewLine, validationResults.Select(x => x.ErrorMessage))}", [validationContext?.MemberName]);
    }
}
