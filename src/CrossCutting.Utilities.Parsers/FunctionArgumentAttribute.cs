namespace CrossCutting.Utilities.Parsers;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
#pragma warning disable CA1019 // Define accessors for attribute arguments
public sealed class FunctionArgumentAttribute : Attribute
{
    public string Name { get; }
    public string TypeName { get; }
    public string Description { get; }
    public bool IsRequired { get; }

    public FunctionArgumentAttribute(string name, string typeName)
    {
        ArgumentGuard.IsNotNull(name, nameof(name));
        ArgumentGuard.IsNotNull(typeName, nameof(typeName));

        Name = name;
        TypeName = typeName;
        Description = string.Empty;
        IsRequired = true;
    }

    public FunctionArgumentAttribute(string name, string typeName, string description)
    {
        ArgumentGuard.IsNotNull(name, nameof(name));
        ArgumentGuard.IsNotNull(typeName, nameof(typeName));
        ArgumentGuard.IsNotNull(description, nameof(description));

        Name = name;
        TypeName = typeName;
        Description = description;
        IsRequired = true;
    }

    public FunctionArgumentAttribute(string name, Type type)
    {
        ArgumentGuard.IsNotNull(name, nameof(name));
        type = ArgumentGuard.IsNotNull(type, nameof(type));

        Name = name;
        TypeName = type.FullName;
        Description = string.Empty;
        IsRequired = true;
    }

    public FunctionArgumentAttribute(string name, Type type, string description)
    {
        ArgumentGuard.IsNotNull(name, nameof(name));
        type = ArgumentGuard.IsNotNull(type, nameof(type));
        ArgumentGuard.IsNotNull(description, nameof(description));

        Name = name;
        TypeName = type.FullName;
        Description = description;
        IsRequired = true;
    }

    public FunctionArgumentAttribute(string name, string typeName, bool isRequired)
    {
        ArgumentGuard.IsNotNull(name, nameof(name));
        ArgumentGuard.IsNotNull(typeName, nameof(typeName));

        Name = name;
        TypeName = typeName;
        Description = string.Empty;
        IsRequired = isRequired;
    }

    public FunctionArgumentAttribute(string name, string typeName, string description, bool isRequired)
    {
        ArgumentGuard.IsNotNull(name, nameof(name));
        ArgumentGuard.IsNotNull(typeName, nameof(typeName));
        ArgumentGuard.IsNotNull(description, nameof(description));

        Name = name;
        TypeName = typeName;
        Description = description;
        IsRequired = isRequired;
    }

    public FunctionArgumentAttribute(string name, Type type, bool isRequired)
    {
        ArgumentGuard.IsNotNull(name, nameof(name));
        type = ArgumentGuard.IsNotNull(type, nameof(type));

        Name = name;
        TypeName = type.FullName;
        Description = string.Empty;
        IsRequired = isRequired;
    }

    public FunctionArgumentAttribute(string name, Type type, string description, bool isRequired)
    {
        ArgumentGuard.IsNotNull(name, nameof(name));
        type = ArgumentGuard.IsNotNull(type, nameof(type));
        ArgumentGuard.IsNotNull(description, nameof(description));

        Name = name;
        TypeName = type.FullName;
        Description = description;
        IsRequired = isRequired;
    }
}
#pragma warning restore CA1019 // Define accessors for attribute arguments
