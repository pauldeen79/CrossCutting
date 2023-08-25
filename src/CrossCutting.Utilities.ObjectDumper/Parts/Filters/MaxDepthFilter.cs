namespace CrossCutting.Utilities.ObjectDumper.Parts.Filters;

public class MaxDepthFilter : IObjectDumperPart
{
    private readonly int _maxDepth;

    public MaxDepthFilter(int maxDepth)
        => _maxDepth = maxDepth;

    public int Order
        => 99;

    public bool Process(object? instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        => false;

    public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source) => source;

    public bool ShouldProcess(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        => currentDepth <= _maxDepth
            || (instance is null || instance is string || instance is Type || instance.GetType().IsPrimitive);

    public bool ShouldProcessProperty(object? instance, PropertyDescriptor propertyDescriptor)
        => true;

    public object? Transform(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        => instance;
}
