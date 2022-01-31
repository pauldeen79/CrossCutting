namespace CrossCutting.Utilities.ObjectDumper.Parts.Types;

public class TypeDumper : IObjectDumperPart
{
    public int Order => 60;

    public bool Process(object? instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
    {
        if (instance is Type t)
        {
            builder.AddSingleValue(t.FullName.FixTypeName().ToString(), instance.GetType());

            return true;
        }

        return false;
    }

    public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source) => source;

    public bool ShouldProcess(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => true;

    public bool ShouldProcessProperty(object? instance, PropertyDescriptor propertyDescriptor) => true;

    public object? Transform(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => instance;
}
