namespace CrossCutting.Utilities.ObjectDumper.Parts.Transforms;

public class OrderByPropertyNameTransform : IObjectDumperPartWithCallback
{
    public IObjectDumperCallback? Callback { get; set; }

    private readonly string? _typeName;
    private readonly Func<Type, bool>? _typeFilter;

    public OrderByPropertyNameTransform(string typeName)
        => _typeName = typeName;

    public OrderByPropertyNameTransform(Func<Type, bool> typeFilter)
        => _typeFilter = typeFilter;

    public int Order
        => 29;

    public bool Process(object? instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        => false;

    public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source)
    {
        source = ArgumentGuard.IsNotNull(source, nameof(source));

        var shouldTransform = false;

        var data = source.ToArray();

        if (_typeName is not null && data.Length != 0 && data[0].ComponentType.FullName == _typeName)
        {
            shouldTransform = true;
        }

        if (_typeFilter is not null && data.Length != 0 && _typeFilter(data[0].ComponentType))
        {
            shouldTransform = true;
        }

        if (!shouldTransform)
        {
            return source;
        }

        return source.OrderBy(p => p.Name);
    }

    public bool ShouldProcess(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        => true;

    public bool ShouldProcessProperty(object? instance, PropertyDescriptor propertyDescriptor)
        => true;

    public object? Transform(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        => instance;
}
