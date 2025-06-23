namespace CrossCutting.Utilities.QueryEvaluator.Validation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class ValidGroupsAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IEnumerable collection)
        {
            return ValidationResult.Success;
        }

        var groupCounter = 0;
        var index = 0;

        foreach (var item in collection)
        {
            if (item == null) continue;

            var type = item.GetType();
            var startGroupProperty = type.GetProperty(nameof(ICondition.StartGroup), BindingFlags.Public | BindingFlags.Instance);
            var endGroupProperty = type.GetProperty(nameof(ICondition.EndGroup), BindingFlags.Public | BindingFlags.Instance);

            if (startGroupProperty is null || endGroupProperty is null)
            {
                return new ValidationResult($"Properties '{nameof(ICondition.StartGroup)}' or '{nameof(ICondition.EndGroup)}' not found on {type.Name}");
            }

#pragma warning disable CS8605 // Unboxing a possibly null value.
            var startGroup = (bool)startGroupProperty.GetValue(item);
            var endGroup = (bool)endGroupProperty.GetValue(item);
#pragma warning restore CS8605 // Unboxing a possibly null value.

            if (startGroup)
            {
                groupCounter++;
            }

            if (endGroup)
            {
                groupCounter--;
            }

            if (groupCounter < 0)
            {
                return new ValidationResult($"EndGroup not valid at index {index}, because there is no corresponding StartGroup");
            }

            index++;
        }

        if (groupCounter == 1)
        {
            return new ValidationResult("Missing EndGroup");
        }
#pragma warning disable S2583 // false positive!
        else if (groupCounter > 1)
#pragma warning restore S2583 // false positive!
        {
            return new ValidationResult($"{groupCounter} missing EndGroups");
        }

        return ValidationResult.Success;
    }
}
