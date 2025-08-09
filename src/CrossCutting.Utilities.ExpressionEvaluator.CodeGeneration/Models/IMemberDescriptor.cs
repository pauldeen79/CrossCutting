namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IMemberDescriptor
{
    [Required] string Name { get; }
    [Required] Type ImplementationType { get; }
    MemberType MemberType { get; }
    Type? ReturnValueType { get; }
    Type? InstanceType { get; }
    [Required(AllowEmptyStrings = true)] string Description { get; }
    bool AllowAllArguments { get; }
    [Required][ValidateObject] IReadOnlyCollection<IMemberDescriptorArgument> Arguments { get; }
    [Required][ValidateObject] IReadOnlyCollection<IMemberDescriptorTypeArgument> TypeArguments { get; }
    [Required][ValidateObject] IReadOnlyCollection<IMemberDescriptorResult> Results { get; }
}
