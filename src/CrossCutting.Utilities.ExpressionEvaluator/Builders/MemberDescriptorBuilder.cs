namespace CrossCutting.Utilities.ExpressionEvaluator.Builders;

public partial class MemberDescriptorBuilder : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => MemberDescriptor.Validate(MemberType, InstanceType, Arguments.Count, TypeArguments.Count);
}
