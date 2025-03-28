namespace CrossCutting.Utilities.ExpressionEvaluator;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
#pragma warning disable CA1019 // Define accessors for attribute arguments
public sealed class FunctionTypeArgumentAttribute : Attribute
{
    public string Name { get; }
    public string Description { get; }

    public FunctionTypeArgumentAttribute(string name)
    {
        ArgumentGuard.IsNotNull(name, nameof(name));

        Name = name;
        Description = string.Empty;
    }

    public FunctionTypeArgumentAttribute(string name, string description)
    {
        ArgumentGuard.IsNotNull(name, nameof(name));
        ArgumentGuard.IsNotNull(description, nameof(description));

        Name = name;
        Description = description;
    }
}
#pragma warning restore CA1019 // Define accessors for attribute arguments
