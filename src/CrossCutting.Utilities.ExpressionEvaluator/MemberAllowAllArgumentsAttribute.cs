namespace CrossCutting.Utilities.ExpressionEvaluator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class MemberAllowAllArgumentsAttribute : Attribute
{
}
