namespace CrossCutting.Utilities.Parsers;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class FunctionIdAttribute : Attribute
{
    public string Id { get; }

    public FunctionIdAttribute(string id)
    {
        ArgumentGuard.IsNotNull(id, nameof(id));

        Id = id;
    }
}
