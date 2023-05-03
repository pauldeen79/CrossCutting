namespace CrossCutting.Utilities.ObjectDumper.Parts.Types;

public class NullDumper : IObjectDumperPart
{
    public int Order => 10;

    public bool Process(object? instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
    {
        if (instance is null)
        {
            builder.AddSingleValue(instance, instanceType);

            return true;
        }

        return false;
    }

    public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source) => source;

    public bool ShouldProcess(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => true;

    public bool ShouldProcessProperty(object? instance, PropertyDescriptor propertyDescriptor) => true;

    public object? Transform(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => instance;
}
