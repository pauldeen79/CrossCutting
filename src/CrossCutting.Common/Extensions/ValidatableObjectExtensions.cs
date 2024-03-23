namespace CrossCutting.Common.Extensions;

public static class ValidatableObjectExtensions
{
    /// <summary>
    /// Validates an IValidatableObject instance using the System.ComponentModel.DataAnnotations.Validator class.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="results">The results.</param>
    /// <param name="value">The value.</param>
    /// <param name="name">The name.</param>
    /// <param name="validationAttributes">The validation attributes.</param>
    /// <returns>
    /// true when valid, false when invalid, null when no validation attributes have been defined.
    /// </returns>
    public static bool? Validate(this IValidatableObject instance,
                                 IList<ValidationResult> results,
                                 object? value,
                                 string name,
                                 IEnumerable<ValidationAttribute>? validationAttributes)
    {
        if (validationAttributes?.Any() == true)
        {
            return Validator.TryValidateValue(value, new ValidationContext(instance, null, null) { MemberName = name }, results, validationAttributes);
        }

        return null;
    }

    /// <summary>
    /// Validates a column of an IValidatableObject for use with IDataErrorInfo.
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="memberName"></param>
    /// <returns>Validation error, or empty when valid.</returns>
    public static string Validate(this IValidatableObject instance, string memberName)
    {
        var errors = instance.Validate(null)
            .Where(vr => vr.MemberNames.Contains(memberName))
            .ToList();

        return ValidateInner(errors);
    }

    /// <summary>
    /// Validates an entire IValidatableObject instance.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <returns>Validation error</returns>
    public static string Validate(this IValidatableObject instance)
    {
        var errors = instance.Validate(null).ToList();

        return ValidateInner(errors);
    }

    /// <summary>
    /// Validates this instance, and throws an exception when validation fails.
    /// </summary>
    /// <typeparam name="TException">The type of the exception.</typeparam>
    /// <param name="instance">The instance.</param>
    public static void Validate<TException>(this IValidatableObject instance)
        where TException : Exception
    {
        var validationErrorMessage = instance.Validate();
        if (!string.IsNullOrEmpty(validationErrorMessage))
        {
            throw (Exception)Activator.CreateInstance(typeof(TException), validationErrorMessage);
        }
    }

    private static string ValidateInner(List<ValidationResult> errors)
    {
        if (errors.Count == 0)
        {
            return string.Empty;
        }

        var sb = new StringBuilder();
        foreach (var error in errors)
        {
            if (sb.Length > 0)
            {
                sb.Append(Environment.NewLine);
            }

            sb.Append(error.ErrorMessage);
        }

        return sb.ToString();
    }
}
