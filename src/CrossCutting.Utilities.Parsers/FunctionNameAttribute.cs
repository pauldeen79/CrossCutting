namespace CrossCutting.Utilities.Parsers;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class FunctionNameAttribute : Attribute
{
    public string Name { get; }

    public FunctionNameAttribute(string name)
    {
        ArgumentGuard.IsNotNull(name, nameof(name));

        Name = name;
    }
}
