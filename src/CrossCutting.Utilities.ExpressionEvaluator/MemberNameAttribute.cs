namespace CrossCutting.Utilities.ExpressionEvaluator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class MemberNameAttribute : Attribute
{
    public string Name { get; }

    public MemberNameAttribute(string name)
    {
        ArgumentGuard.IsNotNull(name, nameof(name));

        Name = name;
    }
}
