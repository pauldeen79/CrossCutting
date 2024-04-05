namespace CrossCutting.Utilities.ObjectDumper.Parts.Filters;

public class PropertyNameFilter : IObjectDumperPart
{
    private readonly IEnumerable<string>? _includePropertyNames;
    private readonly string? _typeName;
    private readonly Func<Type, bool>? _typeFilter;

    public PropertyNameFilter(string includePropertyName) => _includePropertyNames = [includePropertyName];

    public PropertyNameFilter(string includePropertyName, string typeName)
    {
        _includePropertyNames = [includePropertyName];
        _typeName = typeName;
    }

    public PropertyNameFilter(string includePropertyName, Func<Type, bool> typeFilter)
    {
        _includePropertyNames = [includePropertyName];
        _typeFilter = typeFilter;
    }

    public PropertyNameFilter(params string[] includePropertyNames) => _includePropertyNames = includePropertyNames;

    public PropertyNameFilter(IEnumerable<string> includePropertyNames) => _includePropertyNames = includePropertyNames;

    public PropertyNameFilter(IEnumerable<string> includePropertyNames, string typeName)
    {
        _includePropertyNames = includePropertyNames;
        _typeName = typeName;
    }

    public PropertyNameFilter(IEnumerable<string> includePropertyNames, Func<Type, bool> typeFilter)
    {
        _includePropertyNames = includePropertyNames;
        _typeFilter = typeFilter;
    }

    public int Order => 99;

    public bool Process(object? instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        => false;

    public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source) => source;

    public bool ShouldProcess(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => true;

    public bool ShouldProcessProperty(object? instance, PropertyDescriptor propertyDescriptor)
        => _includePropertyNames.Contains(propertyDescriptor.Name)
            && (_typeName is null || propertyDescriptor.ComponentType.FullName == _typeName)
            && (_typeFilter is null || _typeFilter(propertyDescriptor.ComponentType));

    public object? Transform(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => instance;
}
