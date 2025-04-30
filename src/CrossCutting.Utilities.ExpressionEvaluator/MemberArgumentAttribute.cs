namespace CrossCutting.Utilities.ExpressionEvaluator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
#pragma warning disable CA1019 // Define accessors for attribute arguments
public sealed class MemberArgumentAttribute : Attribute
{
    public string Name { get; }
    public Type Type { get; }
    public string Description { get; }
    public bool IsRequired { get; }

    public MemberArgumentAttribute(string name, Type type)
    {
        ArgumentGuard.IsNotNull(name, nameof(name));
        ArgumentGuard.IsNotNull(type, nameof(type));

        Name = name;
        Type = type;
        Description = string.Empty;
        IsRequired = true;
    }

    public MemberArgumentAttribute(string name, Type type, string description)
    {
        ArgumentGuard.IsNotNull(name, nameof(name));
        ArgumentGuard.IsNotNull(type, nameof(type));
        ArgumentGuard.IsNotNull(description, nameof(description));

        Name = name;
        Type = type;
        Description = description;
        IsRequired = true;
    }

    public MemberArgumentAttribute(string name, Type type, bool isRequired)
    {
        ArgumentGuard.IsNotNull(name, nameof(name));
        ArgumentGuard.IsNotNull(type, nameof(type));

        Name = name;
        Type = type;
        Description = string.Empty;
        IsRequired = isRequired;
    }

    public MemberArgumentAttribute(string name, Type type, string description, bool isRequired)
    {
        ArgumentGuard.IsNotNull(name, nameof(name));
        ArgumentGuard.IsNotNull(type, nameof(type));
        ArgumentGuard.IsNotNull(description, nameof(description));

        Name = name;
        Type = type;
        Description = description;
        IsRequired = isRequired;
    }
}
#pragma warning restore CA1019 // Define accessors for attribute arguments
