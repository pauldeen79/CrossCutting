namespace CrossCutting.Utilities.ExpressionEvaluator;

public partial record MemberDescriptor : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => Validate(MemberType, InstanceType, Arguments.Count, TypeArguments.Count);

    public static IEnumerable<ValidationResult> Validate(
        MemberType memberType,
        Type? instanceType,
        int argumentCount,
        int typeArgumentsCount)
    {
        if (memberType == MemberType.Unknown)
        {
            yield return new ValidationResult($"{nameof(MemberType)} cannot be Unknown", [nameof(MemberType)]);
        }

        if (memberType.In(MemberType.Method, MemberType.Property))
        {
            if (instanceType is null)
            {
                yield return new ValidationResult($"{nameof(InstanceType)} is required when MemberType is Method or Property", [nameof(InstanceType)]);
            }

            if (typeArgumentsCount > 0)
            {
                yield return new ValidationResult($"{nameof(TypeArguments)} are not allowed when MemberType is Method or Property", [nameof(TypeArguments)]);
            }
        }
        else
        {
            if (instanceType is not null)
            {
                yield return new ValidationResult($"{nameof(InstanceType)} is not allowed when MemberType is not Method or Property", [nameof(InstanceType)]);
            }
        }

        if (memberType == MemberType.Property && argumentCount > 0)
        {
            yield return new ValidationResult($"{nameof(Arguments)} are not allowed when MemberType is Property", [nameof(Arguments)]);
        }
    }
}
