namespace CrossCutting.Utilities.ObjectDumper.Parts.Filters;

public class PropertyTypeNameExclusionFilter(string skipPropertyTypeName) : IObjectDumperPart
{
    private readonly string _skipPropertyTypeName = skipPropertyTypeName;

    public int Order
        => 99;

    public bool Process(object? instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        => false;

    public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source)
        => source;

    public bool ShouldProcess(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        => true;

    public bool ShouldProcessProperty(object? instance, PropertyDescriptor propertyDescriptor)
        => propertyDescriptor.PropertyType.FullName != _skipPropertyTypeName;

    public object? Transform(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        => instance;
}
