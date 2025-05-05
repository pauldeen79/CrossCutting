namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IFunctionCall
{
    [Required] string Name { get; }
    MemberType MemberType { get; }
    [Required][ValidateObject] IReadOnlyCollection<string> Arguments { get; }
    [Required] IReadOnlyCollection<Type> TypeArguments { get; }
}
