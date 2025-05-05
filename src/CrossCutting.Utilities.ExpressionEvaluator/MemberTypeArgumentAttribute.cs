namespace CrossCutting.Utilities.ExpressionEvaluator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
#pragma warning disable CA1019 // Define accessors for attribute arguments
public sealed class MemberTypeArgumentAttribute : Attribute
{
    public string Name { get; }
    public string Description { get; }

    public MemberTypeArgumentAttribute(string name)
    {
        ArgumentGuard.IsNotNull(name, nameof(name));

        Name = name;
        Description = string.Empty;
    }

    public MemberTypeArgumentAttribute(string name, string description)
    {
        ArgumentGuard.IsNotNull(name, nameof(name));
        ArgumentGuard.IsNotNull(description, nameof(description));

        Name = name;
        Description = description;
    }
}
#pragma warning restore CA1019 // Define accessors for attribute arguments
