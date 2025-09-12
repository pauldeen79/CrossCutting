namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Validation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class AllowedTypesAttribute : ValidationAttribute
{
    public Type[] AllowedTypes { get; }

    public AllowedTypesAttribute(Type[] allowedTypes)
    {
        AllowedTypes = allowedTypes;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null || AllowedTypes.Contains(value.GetType()))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult($"Expression of type {value.GetType()} is not supported");
    }
}
