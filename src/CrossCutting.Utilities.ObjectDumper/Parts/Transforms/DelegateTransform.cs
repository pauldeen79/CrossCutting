namespace CrossCutting.Utilities.ObjectDumper.Parts.Transforms;

public class DelegateTransform(Func<object?, object?> transformDelegate) : IObjectDumperPart
{
    public int Order
        => 99;

    private readonly Func<object?, object?> _transformDelegate = transformDelegate;

    public bool Process(object? instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        => false;

    public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source)
        => source;

    public bool ShouldProcess(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        => true;

    public bool ShouldProcessProperty(object? instance, PropertyDescriptor propertyDescriptor)
        => true;

    public object? Transform(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        => _transformDelegate(instance);
}
